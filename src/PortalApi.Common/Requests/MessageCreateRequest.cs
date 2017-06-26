using FluentValidation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PortalApi.Common
{
    /// <summary>
    /// Message create or reply request
    /// </summary>
    /// Audit/Transaction in stored procedure
    [AuditRequest(AuditGroupEnum.msg, "Msg Create")]
    public class MessageCreateRequest : ApiRequest<ApiResponse>
    {
        /// <summary>
        /// Patient, physician or other mailbox id<br/>
        /// Patient PatId: Z1234<br/>
        /// Physician Mailbox: !123456000, !123456000a (Employee Id)<br/>
        /// SMS Doctor Id: 123456.  Not to be confused with SMSID. Will convert to PSM Mailbox if exists
        /// Special Mailbox: !help, !announcement<br/>
        /// </summary>
        [SwaggerDefaultValue("Z1484859")]
       
        [AuditProperty(AuditFieldEnum.ToId)]
        [Required] 
        public string ToId { get; set; }

        /// <summary>
        /// See ToId
        /// </summary>
        [SwaggerDefaultValue("!help")]
        [AuditProperty(AuditFieldEnum.FromId)]
        [Required] 
        public string FromId { get; set; }

         /// <summary>
        /// When supplied, ReplyTo message becomes beginning of the conversation thread
        /// </summary>
        [JsonIgnore]
        public int ReplyTo { get; set; }

        /// <summary>
        /// Subject up to 90 characters
        /// </summary>
        [SwaggerDefaultValue("Api Test Message")]
        [Required] 
        public string Subject { get; set; }

        [SwaggerDefaultValue("Say something")]
        [Required]
        public string Body { get; set; }
    }

    public class MessageCreateRequestValidator : AbstractValidator<MessageCreateRequest>
    {
        public MessageCreateRequestValidator()
        {
            RuleFor(x => x.ToId).NotEmpty().Matches(@"(^(z|Z)\d{1,23}$)|(^!.{3,23}$)|(^\d{6,6})")
                .WithMessage("{PropertyName} : {PropertyValue} is invalid");

            RuleFor(x => x.FromId).NotEmpty().Matches(@"(^(z|Z)\d{1,23}$)|(^!.{3,23}$)|(^\d{6,6})")
                .WithMessage("{PropertyName} : {PropertyValue} is invalid"); 

            RuleFor(x => x.Subject).NotEmpty().Length(5, 90);
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}
