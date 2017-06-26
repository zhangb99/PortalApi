using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace PortalApi.Web
{
    public class ScopeAttribute : AuthorizeAttribute
    {
        public string Scopes { get; set; }
        public ScopeAttribute(string scopes)
        {
            Scopes = scopes;
        }

        protected override bool IsAuthorized(HttpActionContext context)
        {
            var claims = context.RequestContext.Principal as ClaimsPrincipal;
            // No claims
            if (claims == null) return false;

            var scopes = claims.FindAll("scope").Select(x => x.Value);
            // Insufficient scopes
            if (!Scopes.Split(',').Intersect(scopes).Any()) return false;

            return true;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateErrorResponse(
                          HttpStatusCode.Forbidden, "Insufficient Scopes");
        }
    }
}