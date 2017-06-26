using System;
using MediatR;
using NPoco;
using PortalApi.Common;
using Flurl.Http;
using System.Linq;

namespace PortalApi.Service
{
    public class ValidicUserCreateHandler : IRequestHandler<ValidicUserCreateRequest, ApiResponse>
    {
        private readonly IDatabase _database;
        private readonly IMediator _mediator;

        public ValidicUserCreateHandler(IDatabase database, IMediator mediator)
        {
            _database = database;
            _mediator = mediator;
        }

        public ApiResponse Handle(ValidicUserCreateRequest request)
        {
            var ptlist = _mediator.Send(new PatientInfoRequest(request.Mrn));

            Patient patient = null;

            if (ptlist.Any(x => x.IsPortalUser))
            {
                patient = ptlist.First(x => x.IsPortalUser);
            }
            else if (ptlist.Any())
            {
                patient = ptlist.First();
            }
            else
            {
                return new ApiResponse
                {
                    Status = ResponseStatus.Failed,
                    Message = "Patient does not exist"
                };
            }

            var validicuser = _database.FirstOrDefault<ValidicUser>("WHERE MRN=@0", patient.Mrn);

            if (validicuser != null)
            {
                return new ApiResponse
                {
                    Status = ResponseStatus.Warning,
                    Message = "Patient already provisioned"
                };
            }

            var url = $"{App.Configuration.ValidicAppBase}organizations/{App.Configuration.ValidicMskOrgId}/users.json";

            var result = url.PostJsonAsync(new
            {
                user = new { uid = Guid.NewGuid().ToString() },
                access_token = App.Configuration.ValidicMskAccessToken
            }).ReceiveJson().Result;

            _database.Insert(new ValidicUser
            {
                PatId = patient.PatId,
                Mrn = patient.Mrn,
                uid = result.user.uid,
                _id = result.user._id,
                access_token = result.user.access_token,
                CreatedDate = DateTime.Now
            });

            return new ApiResponse
            {
                Status = ResponseStatus.Success
            };
        }
    }
}
