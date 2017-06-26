using System.Collections.Generic;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace PortalApi.Common
{
    /// <summary>
    /// PatientInfo query model.  Use one of the 3 parameter to search
    /// </summary>
    [AuditRequest(AuditGroupEnum.pat, "Patient Info")]
    public class PatientPortalAccessRequest : ApiRequest<Dictionary<string, bool>>
    {
        public PatientPortalAccessRequest()
        {
            AccessFlag = 33;
            ReminderFlag = 1;
        }

        /// <summary>
        /// whatever of the <see cref="PatientPortalAccessRequest" /> class.
        /// </summary>
        /// <param name="ids">PatId or Mrn to check</param>
        /// <param name="accessFlag">Sum of Proxy Access flag to check msg=1, appt=2, med=4, bill=8, user=16, ppp=32.  Default 33 for PSM and PPP</param>
        /// <param name="reminderFlag">Sum of Reminder preference to check appt=2, bill=32, lab=4, psm=1,psmdept=8,psmgeneral=16</param>
        public PatientPortalAccessRequest(string ids, int accessFlag, int reminderFlag)
        {
            Ids = ids;
            AccessFlag = accessFlag;
            ReminderFlag = reminderFlag;
        }

        /// <summary>
        /// PatIds or Mrns to check.  z123,z456 or 36119464,35102013
        /// </summary>
        [Required]
        [SwaggerDefaultValue("Ids", "z123,z456")]
        public string Ids { get; set; }

        /// <summary>
        /// Sum of Proxy Access flag to check msg=1, appt=2, med=4, bill=8, user=16, ppp=32
        /// </summary>
        [SwaggerDefaultValue("accessFlag", "33")]
        public int AccessFlag { get; set; }

        /// <summary>
        /// Sum of Reminder preference to check appt=2, bill=32, lab=4, psm=1,psmdept=8,psmgeneral=16
        /// </summary>
        [SwaggerDefaultValue("reminderFlag", "1")]
        public int ReminderFlag { get; set; }
    }

    public class PatientPortalAccessRequestValidator : AbstractValidator<PatientPortalAccessRequest>
    {
        public PatientPortalAccessRequestValidator()
        {
            RuleFor(x => x.Ids).NotEmpty();
            RuleFor(x => x.AccessFlag).GreaterThan(0);
            RuleFor(x => x.ReminderFlag).GreaterThan(0);
        }
    }
}
