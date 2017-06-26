using FluentValidation;
using MediatR;
using Newtonsoft.Json;

namespace PortalApi.Common
{
    public abstract class ApiRequest<TResponse> : IRequest<TResponse>
    {
        [AuditProperty(AuditFieldEnum.ClientId)]
        [JsonIgnore]
        public string ClientId { get; set; }

        [AuditProperty(AuditFieldEnum.ResourceOwner)]
        [JsonIgnore]
        public string ResourceOwner { get; set; }
        
        [AuditProperty(AuditFieldEnum.WorkstationInfo)]
        [JsonIgnore]
        public string WorkstationInfo { get; set; }
    }

    //public abstract class PersonValidatorBase<T> : AbstractValidtor<T> where T : IPerson {
    public class ApiRequestValidator<TResponse> : AbstractValidator<ApiRequest<TResponse>> where TResponse : ApiRequest<TResponse>
    {
        public ApiRequestValidator()
        {
            //RuleFor(x => x.ClientId).NotEmpty();
        }
    }
}
