using System.Collections.Generic;
using System.Web.Http;
using PortalApi.Common;

namespace PortalApi.Web.Controllers
{
    /// <summary>
    /// Clinical Research 
    /// </summary>
    public class ClinicalResearchController : BaseApiController
    {
        /// <summary>
        /// Insert/Update Clinical Research Consent Count
        /// </summary>
        /// <remarks>Should update count regardless patient's portal enrollment status</remarks>
        [HttpPost]
        [Route("api/ClinicalResearch/Consent")]
        public ApiResponse Consent(ConsentUpdateRequest request)
        {
            return SendRequest(request);
        }
    }
}
