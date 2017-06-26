using System.Collections.Generic;
using MediatR;
using NPoco;
using PortalApi.Common;

namespace PortalApi.Service
{
    public class MessageListHandler : IRequestHandler<MessageListRequest, List<Message>>
    {
        private readonly IDatabase _database;

        public MessageListHandler(IDatabase database)
        {
            _database = database;
        }
        public List<Message> Handle(MessageListRequest request)
        {
            if (request.MsgId == 0)
            {
                return _database.Fetch<Message>("EXEC Msg_getMsgList @@ToUId=@0, @@FromUId=@0, @@EDGCredential=''",
                    request.PatId);
            }
            else
            {
                return _database.Fetch<Message>("EXEC Msg_getMsgDetails @@PatId=@0, @@MsgId=@1, @@EDGCredential=''",
                    request.PatId, request.MsgId);
            }

        }
    }
}
