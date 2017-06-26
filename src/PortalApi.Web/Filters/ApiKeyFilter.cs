using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Xml;
using ExternalIdentityProvider;
using ExternalIdentityProvider.Schema;
using Newtonsoft.Json;
using PortalApi.Setting;

namespace PortalApi.Web
{
    public class ApiKeyFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Contains("accesstoken"))
            {
                var doc = new XmlDocument();

                try
                {
                    doc.LoadXml(
                        Encoding.UTF8.GetString(
                            Convert.FromBase64String(actionContext.Request.Headers.GetValues("accesstoken").First())));

                    var cert = new X509Certificate2(Path.Combine(
                        AppDomain.CurrentDomain.SetupInformation.ApplicationBase, ApiSettings.Default.CertificatePath),
                       Rijndael.Decrypt(ApiSettings.Default.CertificatePassword));

                    if (CertificateUtility.ValidateX509CertificateSignature(doc, cert))
                    {
                        var assertion =
                            JsonConvert.DeserializeObject<Assertions>(
                                doc.GetElementsByTagName("Assertions")[0].InnerText);
#if DEBUG
                        if (assertion.Issuer.Equals(actionContext.Request.ApiContextInfo().ClientId, StringComparison.CurrentCultureIgnoreCase))
                            return;
#endif
                        if (assertion.TimeStamp > DateTime.UtcNow &&
                            assertion.Issuer.Equals(actionContext.Request.ApiContextInfo().ClientId, StringComparison.CurrentCultureIgnoreCase))
                            return;

                    }
                }
                catch (Exception)
                {
                    actionContext.Response = actionContext.Request.CreateErrorResponse(
                        HttpStatusCode.Forbidden, "Access Token Exception");
                }
            }
            actionContext.Response = actionContext.Request.CreateErrorResponse(
                HttpStatusCode.Forbidden, "Invalid Access Token");
        }
    }
}
