using System.Data.SqlClient;
using MediatR;
using NPoco;
using PortalApi.Common;

namespace PortalApi.Service
{
    public class MessageCreateHandler : IRequestHandler<MessageCreateRequest, ApiResponse>
    {
        private readonly IDatabase _database;

        public MessageCreateHandler(IDatabase database)
        {
            _database = database;
        }
        public ApiResponse Handle(MessageCreateRequest request)
        {
            SqlParameter msgid = new SqlParameter("MsgId", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            _database.Execute(@"EXEC Message_createMsg @@ToUID=@0, @@FromUID=@1, @@Subject=@2, 
@@MsgBody=@3, @@CreatedUID=@4, @@ReplyTo=@5, @@MsgId=@6 OUTPUT, @@ActionRequired = 1, @@EdgCredential=@7",
          request.ToId, request.FromId, request.Subject, request.Body, request.ClientId, request.ReplyTo, msgid, request.WorkstationInfo);

            return new ApiResponse
            {
                Message = "Msg Created",
                ResponseId = msgid.Value.ToString(),
                Status = ResponseStatus.Success
            };
        }
    }
}
