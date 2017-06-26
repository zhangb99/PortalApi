using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using Newtonsoft.Json;

namespace PortalApi.Common
{
    /// <summary>
    /// Update Clinical Research Consent Count
    /// </summary>
    /// Audit/Transaction in stored procedure
    public class ConsentUpdateRequest : ApiRequest<ApiResponse>
    {
        /// <summary>
        /// 8 digit MRN
        /// </summary>
        [SwaggerDefaultValue("36119464")]
        [Required]
        public string Mrn { get; set; }

        /// <summary>
        /// Consent Count
        /// </summary>
        [Required]
        public int ConsentCount { get; set; }
    }

    public class ConsentUpdateRequestValidator : AbstractValidator<ConsentUpdateRequest>
    {
        public ConsentUpdateRequestValidator()
        {
            RuleFor(x => x.Mrn).IsMrn();
            RuleFor(x => x.ConsentCount).NotNull();
        }
    }
}
