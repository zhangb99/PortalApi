using System;

namespace PortalApi.Common
{
    /// <summary>
    /// Message List Item
    /// </summary>
    public class Message
    {
        /// <summary>
        /// MsgId
        /// </summary>
        public int MsgId { get; set; }
        /// <summary>
        /// To Id
        ///     Patient: Z123456
        ///     Mailbox: !help, !123450000
        /// </summary>
        public string ToId { get; set; }
        public string ToName { get; set; }
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
        /// <summary>
        /// Inbox, Sent, Trash
        /// </summary>
        public string Folder { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public DateTime? DeletedDate { get; set; }
        /// <summary>
        /// MsgBody is only populated when returning as MsgDetail list.
        /// </summary>
        public string MsgBody { get; set; }
        /// <summary>
        /// Should not allow reply
        /// </summary>
        public bool NoReply { get; set; }
        /// <summary>
        /// Mailbox no longer active
        /// </summary>
        public bool MbxActive { get; set; }

        public Guid? AttachmentId { get; set; }
        public string AttachmentDetails { get; set; }
        public bool AttachmentExpired { get; set; }
    }
}
