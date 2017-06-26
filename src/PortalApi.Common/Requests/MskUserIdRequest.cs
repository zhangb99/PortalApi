using FluentValidation;

namespace PortalApi.Common
{
    /// <summary>
    /// Search MSK User by various Ids
    /// </summary>
    [AuditRequest(AuditGroupEnum.msk, "MskUserId")]
    public class MskUserIdRequest : ApiRequest<MskUserId>
    {

        /// <summary>
        /// Search user by id
        /// </summary>
        /// <param name="idType">MskUserIdTypeEnum</param>
        /// <param name="id">id</param>
        public MskUserIdRequest(MskUserIdTypeEnum idType, string id)
        {
            IdType = idType;
            Id = id;
        }

        [AuditProperty(AuditFieldEnum.Blob)]
        public MskUserIdTypeEnum IdType { get; set; }

        [AuditProperty(AuditFieldEnum.Blob)]
        public string Id { get; set; } 
    }

    public enum MskUserIdTypeEnum
    {
        EmpId,
        NtId,
        SmsDoctorId,
        HisId,
        Email,
        PsmId
    }

    public class MskUserIdRequestRequestValidator : AbstractValidator<MskUserIdRequest>
    {
        public MskUserIdRequestRequestValidator()
        {
            RuleFor(x => x.IdType).NotNull();
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
