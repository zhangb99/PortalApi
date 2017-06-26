using FluentValidation;

namespace PortalApi.Common
{
    /// <summary>
    /// Get Validic user details and app list
    /// </summary>
    [AuditRequest(AuditGroupEnum.pat, "Validic User Details")]
    public class ValidicUserRequest : ApiRequest<ValidicUser>
    {
        /// <summary>
        /// whatever of the <see cref="ValidicUserRequest"/> class.
        /// </summary>
        /// <param name="id">PatId, Mrn, uid or _id</param>
        public ValidicUserRequest(string id)
        {
            Id = id;
        }

        /// <summary>
        /// Portal Validic uid: f8e05adb-7cc7-41be-9b97-1450dc9b6a07
        /// Validic _id: 592eea15b8004add8a0105c8
        /// MRN: 12345678
        /// PatId: Z123
        /// </summary>
        [AuditProperty(AuditFieldEnum.Blob)]
        public string Id { get; set; }
    }

    public class ValidicUserRequestValidator : AbstractValidator<ValidicUserRequest>
    {
        public ValidicUserRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
