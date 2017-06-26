using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using Microsoft.Owin;

namespace PortalApi.Web
{
    public static class ApiContextExtension
    {
        public static ApiContextInfo ApiContextInfo()
        {
            var context = HttpContext.Current.Request.GetOwinContext();

            var principal = context.Authentication.User;

            if (principal == null || !principal.Identity.IsAuthenticated)
            {
                return new ApiContextInfo
                {
                    //ClientIp = ((OwinContext) request.Properties["MS_OwinContext"]).Request.RemoteIpAddress,
                    ClientIp = context.Request.RemoteIpAddress ?? ""
                };
            }


            return new ApiContextInfo
            {
                ClientId = principal.FindFirst("client_id").Value,
                UserId = principal.FindFirst("sub").Value,
                ClientIp = context.Request.RemoteIpAddress ?? "",
                Scopes = principal.FindAll("scope").Select(x => x.Value).ToArray()
                //Scopes = principal.FindAll("scope")
                //ClientIp = ((OwinContext) request.Properties["MS_OwinContext"]).Request.RemoteIpAddress
                //ClientIp = ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.ServerVariables["REMOTE_ADDR"]
            };
        }
    }

    public class ApiContextInfo
    {
        public string ClientIp { get; set; }
        public string ClientId { get; set; }
        public string UserId { get; set; }
        public string[] Scopes { get; set; }
    }
}