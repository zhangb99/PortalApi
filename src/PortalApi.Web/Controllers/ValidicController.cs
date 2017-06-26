using PortalApi.Common;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace PortalApi.Web.Controllers
{
    public class ValidicController : BaseApiController
    {
        /// <summary>
        /// Get list of all provisioned MSK patients
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<ValidicUser>))]
        public List<ValidicUser> GetAll()
        {
            return SendRequest(new ValidicUserListRequest());
        }

        /// <summary>
        /// Get detailed Validic user information and synced app list
        /// </summary>
        /// <param name="id">PatId, Mrn, Validic Portal Id or Validic _id</param>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ValidicUser))]
        [SwaggerDefaultValue("Id", "36119464")]
        [Route("api/Validic/{id}")]
        public ValidicUser Get(string id)
        {
            return SendRequest(new ValidicUserRequest(id));
        }

        /// <summary>
        /// Provisioning Validic user
        /// </summary>
        /// <returns></returns>
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ApiResponse))]
        public ApiResponse Post(ValidicUserCreateRequest request)
        {
            return SendRequest(request);
        }
    }
}
