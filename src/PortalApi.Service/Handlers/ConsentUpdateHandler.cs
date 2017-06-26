using System.Data.SqlClient;
using MediatR;
using NPoco;
using PortalApi.Common;

namespace PortalApi.Service
{
    public class ConsentUpdateHandler : IRequestHandler<ConsentUpdateRequest, ApiResponse>
    {
        private readonly IDatabase _database;

        public ConsentUpdateHandler(IDatabase database)
        {
            _database = database;
        }
        public ApiResponse Handle(ConsentUpdateRequest request)
        {
            var consent = _database.SingleOrDefaultById<EConsent>(request.Mrn);

            if (consent != null)
            {
                consent.ConsentCount = request.ConsentCount;
                _database.Update(consent);
            }
            else
            {
                _database.Insert(new EConsent
                {
                    Mrn = request.Mrn,
                    ConsentCount = request.ConsentCount
                });
            }

            return new ApiResponse
            {
                Message = "Updated",
                Status = ResponseStatus.Success
            };
        }

        [TableName("EConsent"), PrimaryKey("Mrn", AutoIncrement = false)]
        private class EConsent
        {
            public string Mrn { get; set; }
            public int ConsentCount { get; set; }
        }
    }
}
