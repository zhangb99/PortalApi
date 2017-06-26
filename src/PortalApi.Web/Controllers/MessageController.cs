using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text.RegularExpressions;
using System.Web.Http;
using PortalApi.Common;

namespace PortalApi.Web.Controllers
{
    /// <summary>
    /// Portal Secure Messaging 
    /// </summary>
    public class MessageController : BaseApiController
    {
        /// <summary>
        /// Get message list by PatId
        /// </summary>
        /// <param name="patId">Z1234</param>
        [Route("api/Message/{patId}")]
        [SwaggerDefaultValue("patId", "Z102783")]
        public List<Message> Get(string patId)
        {
            return SendRequest(new MessageListRequest(patId));
        }

        /// <summary>
        /// Get message detail list by PatId.  <br />
        /// Portal message details is a list of threaded conversation, <br />
        /// each message does not quote previous message
        /// </summary>
        /// <param name="patId">Z1234</param>
        /// <param name="msgId">MsgId as the start of the thread</param>
        [Route("api/Message/{patId}/{msgId}")]
        [SwaggerDefaultValue("patId", "Z102783")]
        [SwaggerDefaultValue("msgId", "5098")]
        public List<Message> Get(string patId, int msgId)
        {
            return SendRequest(new MessageListRequest(patId, msgId));
        }

        /// <summary>
        /// Create or Reply to a message <br/>
        /// </summary>
        /// <param name="message">MessageCreateRequest</param>
        [HttpPost]
        public ApiResponse Post(MessageCreateRequest message)
        {
            if (Regex.IsMatch(message.ToId, @"^\d{6,6}$"))
            {
                var mskuserid = SendRequest(new MskUserIdRequest(MskUserIdTypeEnum.SmsDoctorId, message.ToId));
                if (mskuserid == null || string.IsNullOrWhiteSpace(mskuserid.SmsDoctorId))
                {
                    return new ApiResponse
                    {
                        Message = "No PSM Mailbox found for the given SMS Doctor Id",
                        Status = ResponseStatus.NotFound
                    };
                }

                message.ToId = mskuserid.PsmId;
            }
            else if (Regex.IsMatch(message.FromId, @"^\d{6,6}$"))
            {
                var mskuserid = SendRequest(new MskUserIdRequest(MskUserIdTypeEnum.SmsDoctorId, message.FromId));

                if (mskuserid == null || string.IsNullOrWhiteSpace(mskuserid.SmsDoctorId))
                {
                    return new ApiResponse
                    {
                        Message = "No PSM Mailbox found for the given SMS Doctor Id",
                        Status = ResponseStatus.NotFound
                    };
                }

                message.FromId = mskuserid.PsmId;
            }

            return SendRequest(message);
        }

        
    }
}
