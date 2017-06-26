using System;
using System.Security.Claims;
using System.Web.Http;
using IdentityModel.Client;
using MediatR;
using PortalApi.Common;

namespace PortalApi.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : ApiController
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// PortalApi Ping Heartbeat.
        /// </summary>
        public string Get()
        {
            return _mediator.Send(new PingRequest());
        }
    }
}
