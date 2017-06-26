using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace PortalApi.Web
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext context)
        {
            // Not authenticated (no token)
            if (!context.RequestContext.Principal.Identity.IsAuthenticated) return false;

            var claims = context.RequestContext.Principal as ClaimsPrincipal;
            // No claims
            if (claims == null) return false;

            var clientid = claims.FindFirst("client_id");
            // Not on client list
            if (clientid == null || !MyClients().Any(x => string.Equals(x, clientid.Value))) return false;

            var user = claims.FindFirst("sub");
            // Not on user list
            if (user == null || !MyUsers().Any(x => string.Equals(x, user.Value))) return false;

            // Base checks Users and Roles if needed
            return base.IsAuthorized(context);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateErrorResponse(
                          HttpStatusCode.Forbidden, "Access Denied");
        }

        private string[] MyClients()
        {
            return new[] { "client_portalapi_swagger" };
        }

        private string[] MyUsers()
        {
            return new[] { "res_patportalapppool", "res_patportaladmpool", "student1" };
        }
    }
}