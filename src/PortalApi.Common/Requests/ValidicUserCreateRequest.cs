using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentValidation;
using System;

namespace PortalApi.Common
{
    /// <summary>
    /// Provisioning Validic user
    /// </summary>
    [AuditRequest(AuditGroupEnum.pat, "Provisioning Validic user")]
    public class ValidicUserCreateRequest : ApiRequest<ApiResponse>
    {
        public ValidicUserCreateRequest(string mrn)
        {
            Mrn = mrn;
        }

        /// <summary>
        /// MRN: 12345678
        /// </summary>
        [AuditProperty(AuditFieldEnum.Mrn)]
        public string Mrn { get; set; }
    }

    public class ValidicUserCreateRequestValidator : AbstractValidator<ValidicUserCreateRequest>
    {
        public ValidicUserCreateRequestValidator()
        {
            RuleFor(x => x.Mrn).NotEmpty().IsMrn();
        }
    }
}
