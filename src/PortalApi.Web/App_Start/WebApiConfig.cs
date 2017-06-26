using System.Linq;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using PortalApi.Web.Filters;

namespace PortalApi.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // excption handler TBD
            //// Message Handler
            ////config.MessageHandlers.Add(new ApiKeyHandler());

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;

            config.Services.Replace(typeof(IExceptionHandler), new ApiExceptionHandler());
            config.Services.Replace(typeof(IExceptionLogger), new ApiExceptionLogger());

            // Global filter
            //config.Filters.Add(new ApiKeyFilter());
            //config.ParameterBindingRules.Insert(0, PostParameterBinding.HookupParameterBinding);

            config.Formatters.XmlFormatter.SupportedMediaTypes
                .Remove(config.Formatters.XmlFormatter.SupportedMediaTypes
                .FirstOrDefault(t => t.MediaType == "application/xml"));

            config.Formatters.JsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            config.Formatters.JsonFormatter.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        }
    }
}
