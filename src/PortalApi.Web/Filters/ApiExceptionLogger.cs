using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using PortalApi.Common;

namespace PortalApi.Web.Filters
{
    /// <summary>
    /// Log exceptions via NLog, except validation exception
    /// </summary>
    public class ApiExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
//#if DEBUG
//            return;
//#endif

            var exceptioninfo = new StringBuilder("Request Ids: ");

            var exclusions = new[] { "password", "clientsecret" };

            ((ApiController)context.ExceptionContext.ControllerContext.Controller).ActionContext.ActionArguments
                .Where(x => !exclusions.Contains(x.Key.ToLower())).ToList().ForEach(x =>
                {
                    if (IsSubclassOfRawGeneric(typeof(ApiRequest<>), x.Value.GetType()))
                    {
                        exceptioninfo.Append(
                            JsonConvert.SerializeObject(x.Value, new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore,
                                ContractResolver = new LoggingContractResolver(),
                            }));

                    }
                    else
                    {
                        exceptioninfo.Append(string.Concat(x.Key, ":", x.Value.ToString(), "   "));
                    }
                });

            var contextinfo = ApiContextExtension.ApiContextInfo();

            var eventInfo = new LogEventInfo()
            {
                LoggerName = "PortalApi",
                Level = (context.Exception is ValidationException) ? LogLevel.Warn : LogLevel.Error,
                Message = exceptioninfo.Append("\r\n\r\nException Details: " + context.Exception).ToString(),
                Exception = context.Exception,
                TimeStamp = DateTime.Now
            };

            eventInfo.Properties.Add("ClientId", contextinfo.ClientId);
            eventInfo.Properties.Add("ClientIp", contextinfo.ClientIp);
            eventInfo.Properties.Add("UserId", contextinfo.UserId);
            eventInfo.Properties.Add("Request", context.Request.RequestUri.LocalPath);
            eventInfo.Properties.Add("Method", context.Request.Method.Method);

            LogManager.GetLogger("apilog").Log(eventInfo);
        }

        static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }

    internal class LoggingContractResolver : DefaultContractResolver
    {
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            properties.ToList().ForEach(x =>
            {
                x.ShouldSerialize =
                    objc => x.AttributeProvider.GetAttributes(typeof(AuditPropertyAttribute), true).Any();
            });

            return properties;
        }
    }


}
