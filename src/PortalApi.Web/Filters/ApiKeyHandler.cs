using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Xml;
using ExternalIdentityProvider;

namespace PortalApi.Web
{
    public class ApiKeyHandler : DelegatingHandler
    {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool isValidAPIKey = false;

            //Validate that the api key exists, and if so, validate
            string token = request.Headers.Contains("accesstoken")
                ? request.Headers.GetValues("accesstoken").FirstOrDefault()
                : "";

            var doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(Convert.FromBase64String(token)));

            try
            {
                var cert = new X509Certificate2(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\bin\\test.pfx", "pass");
                isValidAPIKey = CertificateUtility.ValidateX509CertificateSignature(doc, cert);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            

            //If the key is not valid, return an http status code. This message could, of course, be localized using resources.
            if (!isValidAPIKey)
                return request.CreateResponse(HttpStatusCode.Forbidden, "Bad API Key");

            //Allow the request to process further down the pipeline
            var response = await base.SendAsync(request, cancellationToken);

            //Return the response back up the chain
            return response;
        }
    }
}
