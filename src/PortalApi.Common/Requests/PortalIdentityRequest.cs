using System.Collections.Generic;

namespace PortalApi.Common
{
    /// <summary>
    /// UserId/Password
    /// </summary>
    [AuditRequest(AuditGroupEnum.msg, "Create Msg")]
    //[Transaction]
    public class PortalIdentityRequest : ApiRequest<ApiResponse>
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
