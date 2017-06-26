using System.Net;
using System.Web.Http;
using PortalApi.Common;
using Swashbuckle.Swagger.Annotations;

namespace PortalApi.Web.Controllers
{
    public class MskUserInfoController : BaseApiController
    {
        /// <summary>
        /// Get MSKUser Id Mapping
        /// </summary>
        /// <param name="idType">MskUserIdTypeEnum</param>
        /// <param name="idValue">Id to search</param>
        [Route("api/MskUserInfo")]
        [Route("api/MskUserInfo/{idType}/{idValue}")]
        public MskUserId GetById(MskUserIdTypeEnum idType, string idValue)
        {
            return SendRequest(new MskUserIdRequest(idType, idValue));
        }
    }
}
