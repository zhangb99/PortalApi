using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Net;
using System.Web.Helpers;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;
using PortalApi.Common;

[assembly: OwinStartup(typeof(PortalApi.Web.Startup))]
namespace PortalApi.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = App.Configuration.SsoAuthority,
                RequiredScopes = App.Configuration.RequiredScope.Split(';')
            });

            SimpleInjectorWebApi.Initialize(app);
        }
    }
}
