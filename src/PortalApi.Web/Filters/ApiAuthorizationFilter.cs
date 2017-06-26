using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NPoco;
using PortalApi.Common;
using PortalApi.Service;

namespace PortalApi.Web
{
    public class ApiAuthorizationFilter : ActionFilterAttribute
    {
        private readonly IDatabase _database;
        private readonly ICacheService _cache;


        public ApiAuthorizationFilter(IDatabase database, ICacheService cache)
        {
            _database = database;
            _cache = cache;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
#if DEBUG
            return;
#endif
            if (actionContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                try
                {
                    var clientlist = _cache.Get("apiclientlist", () =>
                        _database.Fetch<ApiAuth>().Where(x => x.Type == "client").Select(x => x.Name).ToList(), App.Configuration.ClientConfigCacheMinutes);

                    var ownerlist = _cache.Get("apiownerlist", () =>
                        _database.Fetch<ApiAuth>().Where(x => x.Type == "owner").Select(x => x.Name).ToList(), App.Configuration.ClientConfigCacheMinutes);

                    var info = ApiContextExtension.ApiContextInfo();

                    if (clientlist.Any(x => string.Equals(x, info.ClientId, StringComparison.OrdinalIgnoreCase))
                        && ownerlist.Any(x => string.Equals(x, info.UserId, StringComparison.OrdinalIgnoreCase)))
                    {
                        base.OnActionExecuting(actionContext);
                    }
                    else
                    {
                        actionContext.Response = actionContext.Request.CreateErrorResponse(
                            HttpStatusCode.Forbidden, "Unauthorized client or user");
                    }
                }
                catch (Exception ex)
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.BadRequest, "Unable to verify authorization.  " + ex.Message);
                }
            }
            else if (!actionContext.ControllerContext.ControllerDescriptor
                .GetCustomAttributes<AllowAnonymousAttribute>().Any())
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized, "Authorization has been denied for this request.");
            }
        }

        class ApiAuth
        {
            public string Type { get; set; }
            public string Name { get; set; }
        }
    }
}
