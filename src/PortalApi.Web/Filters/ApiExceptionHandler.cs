using System.IdentityModel.Claims;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;
using System.Web.Routing;
using FluentValidation;

namespace PortalApi.Web.Filters
{
    public class ApiExceptionHandler : ExceptionHandler
    {
        public override void Handle(ExceptionHandlerContext context)
        {
            var exception = context.Exception as ValidationException;
            if (exception != null)
            {
                var errors = new ModelStateDictionary();
                foreach (var error in exception.Errors)
                {
                    errors.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                context.Result = new ResponseMessageResult(
                    context.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errors));
            }
            else
            {
                var info = ApiContextExtension.ApiContextInfo();

                context.Result =
                    new ResponseMessageResult(context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                        new
                        {
                            context.Exception.Message,
                            info.ClientId,
                            info.UserId,
                            info.ClientIp,
                            Request = context.Request.RequestUri.LocalPath,
                            context.Request.Method.Method,
                            Exception = context.Exception.ToString()
                        }));
            }
        }
    }
}