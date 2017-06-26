using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentValidation;

namespace PortalApi.Common
{
    /// <summary>
    /// PatientInfo query model.  Use one of the 3 parameter to search
    /// </summary>
    [AuditRequest(AuditGroupEnum.pat, "Patient Info")]
    public class PatientInfoRequest : ApiRequest<List<Patient>>
    {
        public PatientInfoRequest()
        {
        }

        /// <summary>
        /// whatever of the <see cref="PatientInfoRequest"/> class.
        /// </summary>
        /// <param name="id">PatId or Mrn</param>
        public PatientInfoRequest(string id)
        {
            if (Regex.IsMatch(id, @"^\d{8,8}$"))
                Mrn = id;
            else
                PatId = id;
        }

        /// <summary>
        /// PatId: Z1234
        /// </summary>
        [AuditProperty(AuditFieldEnum.PatId)]
        public string PatId { get; set; }

        /// <summary>
        /// MRN: 12345678
        /// </summary>
        [AuditProperty(AuditFieldEnum.Mrn)]

        public string Mrn { get; set; }

        /// <summary>
        /// User Name: Smith, Jo 
        /// </summary>
        [AuditProperty(AuditFieldEnum.Blob)]
        public string Name { get; set; }
    }

    public class PatientInfoRequestValidator : AbstractValidator<PatientInfoRequest>
    {
        public PatientInfoRequestValidator()
        {
            RuleFor(x => x.PatId).Matches(@"^(z|Z)\d{1,23}$").When(x => !string.IsNullOrEmpty(x.PatId))
                .WithMessage("{PropertyName} {PropertyValue} is invalid");
            
            RuleFor(x => x.Mrn).Matches(@"^\d{8,8}$").When(x => !string.IsNullOrEmpty(x.Mrn))
                .WithMessage("{PropertyName} : {PropertyValue} is invalid");

            RuleFor(x => x.Name).Length(5, 20).When(x => !string.IsNullOrEmpty(x.Name)); ;
            RuleFor(pt => pt)
                .Must(x => !string.IsNullOrEmpty(x.PatId)
                           || !string.IsNullOrEmpty(x.Mrn) || !string.IsNullOrEmpty(x.Name))
                .WithMessage("Enter MRN, User Name or PatId");
        }
    }
}
