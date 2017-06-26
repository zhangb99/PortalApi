using System.Web.Http;
using PortalApi.Common;

namespace PortalApi.Web.Controllers
{
    /// <summary>
    /// Verify user identity against Patient Portal and other backend sources
    /// </summary>
    public class IdentityController : BaseApiController
    {
        /// <summary>
        /// Verify Portal User Id/Password.  <br/>
        /// Note: This does not authorize user access to patient information, <br/>
        /// which requires further verification of proxy access<br/>
        /// </summary>
        [HttpPost]
        [Route("api/Identity/Portal")]
        public ApiResponse Post(PortalIdentityRequest request)
        {
            return new ApiResponse();
        }

        /// <summary>
        /// Verify if user has appointment at clinical kiosk location
        /// </summary>
        /// <remarks>Testing require a patient with appointment today at specific location</remarks>
        [HttpPost]
        [Route("api/Identity/Kiosk")]
        public KioskIdentityResponse Post(KioskIdentityRequest request)
        {
            return SendRequest(request);
        }
    }
}
