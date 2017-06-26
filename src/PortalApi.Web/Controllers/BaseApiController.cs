using System.Net;
using System.Web.Http;
using MediatR;
using PortalApi.Common;
using Swashbuckle.Swagger.Annotations;

namespace PortalApi.Web.Controllers
{
    [SwaggerResponse(HttpStatusCode.OK)]
    [SwaggerResponse(HttpStatusCode.BadRequest, "Request is invalid")]
    [SwaggerResponse(HttpStatusCode.Unauthorized, "Missing or invalid access token")]
    [SwaggerResponse(HttpStatusCode.Forbidden, "Client or resource account not authorized")]
    [SwaggerResponse(HttpStatusCode.InternalServerError, "Unknow error")]
    public class BaseApiController : ApiController
    {
        public IMediator _mediator;

        protected TResponse SendRequest<TResponse>(IRequest<TResponse> request)
        {
            var apirequest = request as ApiRequest<TResponse>;

            if (apirequest != null)
            {
                var info = ApiContextExtension.ApiContextInfo();
                apirequest.ClientId = info.ClientId;
                apirequest.ResourceOwner = info.UserId;
                apirequest.WorkstationInfo = info.ClientIp;
            }

            return _mediator.Send(request);
        }
    }
}