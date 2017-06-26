using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace PortalApi.Common
{
    /// <summary>
    /// Kiosk and Patient Info
    /// </summary>
    public class KioskIdentityRequest : ApiRequest<KioskIdentityResponse>
    {
        /// <summary>
        /// Date of birth
        /// </summary>
        [Required]
        [SwaggerDefaultValue("1996-1-6")]
        public DateTime Dob { get; set; }

        [Required]
        [SwaggerDefaultValue("10011")]
        public string ZipCode { get; set; }

        /// <summary>
        /// Tie breaker if two users with same Zip/Dob having appointment at the same location same day.
        /// </summary>
        [SwaggerDefaultValue("718-123-4567")]
        public string Phone { get; set; }

        /// <summary>
        /// Location is set by application for each kiosk location.  They must be enabled in Epic if checkin is needed
        /// </summary>
        [Required]
        [SwaggerDefaultValue("MSK-53rd St")]
        public string Location { get; set; }

        /// <summary>
        /// For Kiosk/Checkin, this should be set by application.
        /// </summary>
        [Required]
        [SwaggerDefaultValue("2017-1-5")]
        public DateTime Today { get; set; }
    }

    public class KioskIdentityRequestValidator : AbstractValidator<KioskIdentityRequest>
    {
        public KioskIdentityRequestValidator()
        {
            RuleFor(x => x.Dob).NotEmpty();
            RuleFor(x => x.ZipCode).NotEmpty();
            RuleFor(x => x.Location).NotEmpty();
            RuleFor(x => x.Today).NotEmpty();
        }
    }

    public class KioskIdentityResponse : ApiResponse
    {
        public KioskPatient Patient { get; set; }
    }
}
