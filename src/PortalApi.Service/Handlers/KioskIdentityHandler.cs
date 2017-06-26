using MediatR;
using NPoco;
using PortalApi.Common;

namespace PortalApi.Service
{
    public class KioskIdentityHandler : IRequestHandler<KioskIdentityRequest, KioskIdentityResponse>
    {
        private readonly IDatabase _database;

        public KioskIdentityHandler(IDatabase database)
        {
            _database = database;
        }
        public KioskIdentityResponse Handle(KioskIdentityRequest request)
        {
            var users = _database.FetchOneToMany<KioskPatient>(x => x.Appointments,
                "EXEC [EWS_KioskUserApptInfo] @@Dob=@0, @@ZipCode=@1, @@PhoneNum=@2, @@FromDate=@3, @@AptStatus='1,6', @@EDGCredentials='api'"
                , request.Dob.ToShortDateString(), request.ZipCode, request.Phone, request.Today);

            return users.GetValidKioskUser(request.Location);
        }
    }
}
