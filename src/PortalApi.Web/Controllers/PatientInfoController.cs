using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using PortalApi.Common;
using SwaggerExtensions;

namespace PortalApi.Web.Controllers
{
    /// <summary>
    /// Get patient by MRN, PatId; or search by Last, First name.
    /// </summary>
    public class PatientInfoController : BaseApiController
    {
        /// <summary>
        /// Get Patient by MRN or PatId
        /// </summary>
        /// <param name="id">MRN: 12345678 or PatId: Z1234</param>
        [SwaggerDefaultValue("id", "36119464")]
        public Patient Get(string id)
        {
            return SendRequest(new PatientInfoRequest(id)).FirstOrDefault();
        }

        /// <summary>
        /// Search patient by name
        /// </summary>
        /// <param name="name">patient name</param>
        [Route("api/PatientInfo")]
        [SwaggerDefaultValue("name", "portal, ja")]
        public List<Patient> GetByName(string name)
        {
            return SendRequest(new PatientInfoRequest { Name = name });
        }

        /// <summary>
        /// Check patient's portal user access
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [Route("api/PatientInfo/PortalAccess")]
        public Dictionary<string, bool> PortalAccess(PatientPortalAccessRequest request)
        {
            return SendRequest(request);
        }
    }
}
