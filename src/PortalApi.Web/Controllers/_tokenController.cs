using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using IdentityModel;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using PortalApi.Common;


namespace PortalApi.Web.Controllers
{
    [RoutePrefix("api/token")]
    [AllowAnonymous]
    public class _tokenController : ApiController
    {
        /// <summary>
        /// Generates new secret for serviceclient.
        /// </summary>
        /// <remarks>
        /// This is good, so I was told <br/><br/>
        /// &#160;&#160;&#160;var buffer = new byte[length];<br/>
        /// &#160;&#160;&#160;new RNGCryptoServiceProvider().GetBytes(buffer);<br/>
        /// &#160;&#160;&#160;return Convert.ToBase64String(buffer);<br/>
        /// </remarks>
        [SwaggerDefaultValue("length", "32", SwaggerEnv = SwaggerEnvEnum.ALL)]
        [Route("ClientSecret")]
        public string GenerateSecret(int length)
        {
            var buffer = new byte[length];
            new RNGCryptoServiceProvider().GetBytes(buffer);
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Get ResouseOwner (user) token, allowing client accessing api on behalf of user
        /// </summary>
        /// <remarks>
        /// Copy the token string to the bearer token textbox above to access secured resources<br/><br/>
        /// SsoAd Token Endpoint<br/>
        /// &#160;&#160;&#160;TEST: https://portalsso.mskcc.org/TEST/SsoAd/connect/token <br/>
        /// &#160;&#160;&#160;PROD: https://portalsso.mskcc.org/SsoAd/connect/token <br/>
        /// </remarks>
        [SwaggerDefaultValue("scope", "read write", SwaggerEnv = SwaggerEnvEnum.ALL)]
        [SwaggerDefaultValue("ntid", "student1", SwaggerEnv = SwaggerEnvEnum.DEBUG)]
        [SwaggerDefaultValue("clientId", "client_portalapi_swagger", SwaggerEnv = SwaggerEnvEnum.DEBUG)]
        [SwaggerDefaultValue("clientSecret", "secret", SwaggerEnv = SwaggerEnvEnum.DEBUG)]
        [SwaggerDefaultValue("password", "student1", SwaggerEnv = SwaggerEnvEnum.DEBUG)]
        [Route("ResourceOwnerToken")]
        public HttpResponseMessage ResourceOwnerToken(string scope, string clientId, string clientSecret, string ntid, string password) //, string optionals)
        {
            ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            
            var client = new TokenClient(App.Configuration.SsoAuthority + "/connect/token", clientId, clientSecret);

            var token = client.RequestResourceOwnerPasswordAsync(ntid, password, scope).Result;
            //var token = client.RequestResourceOwnerPasswordAsync(ntid, password, scope, new { acr_values = optionals }).Result;

            return new HttpResponseMessage
            {
                Content = new StringContent(token.Json.ToString(), Encoding.UTF8, "text/html")
            };
        }

        /// <summary>
        /// Decode Jwt token and see what's inside
        /// </summary>
        /// <remarks>Paste access token in the upper right bearer token box</remarks>
        [Route("Decode")]
        public HttpResponseMessage Decode()
        {
            IEnumerable<string> headers;
            Request.Headers.TryGetValues("Authorization", out headers);

            string token = headers.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(token)) return null;

            token = token.Replace("Bearer ", "");

            var result = "Invalid";
            if (token.Contains("."))
            {
                result = "Access Token (decoded):\r\n";

                var parts = token.Split('.');
                var header = parts[0];
                var claims = parts[1];

                result += JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(header)));
                result += JObject.Parse(Encoding.UTF8.GetString(Base64Url.Decode(claims)));
            }

            return new HttpResponseMessage
            {
                Content = new StringContent(result, Encoding.UTF8, "text/html")
            };
        }

        /// <summary>
        /// View Current Authenitcated User based on Token
        /// </summary>
        /// <remarks>Paste token in the upper righter bearer token box and try.  This is what you see on service side</remarks>
        [Authorize]
        public IHttpActionResult ClaimsPricipal()
        {
            var user = User as ClaimsPrincipal;

            if (user == null || user.Claims == null) return Json("ClaimsPrincipal is null");

            var claims = from c in user.Claims
                         select new
                         {
                             type = c.Type,
                             value = c.Value
                         };

            return Json(claims);
        }
    }
}