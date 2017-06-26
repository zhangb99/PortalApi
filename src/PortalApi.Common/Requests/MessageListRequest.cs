using System.Collections.Generic;
using FluentValidation;

namespace PortalApi.Common
{
    /// <summary>
    /// Message create or reply request
    /// </summary>
    [AuditRequest(AuditGroupEnum.msg, "Message List/Details")]
    public class MessageListRequest : ApiRequest<List<Message>>
    {
        public MessageListRequest(string patId)
        {
            PatId = patId;
        }

        public MessageListRequest(string patId, int msgId)
        {
            PatId = patId;
            MsgId = msgId;
        }

        [AuditProperty(AuditFieldEnum.PatId)]
        public string PatId { get; set; }

        [AuditProperty(AuditFieldEnum.MsgId)]
        public int MsgId { get; set; }
    }

    public class MessageListRequestValidator : AbstractValidator<MessageListRequest>
    {
        public MessageListRequestValidator()
        {
            RuleFor(x => x.PatId).NotEmpty().IsPatId();
        }
    }
}
