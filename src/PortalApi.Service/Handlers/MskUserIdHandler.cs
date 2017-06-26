using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NPoco;
using PortalApi.Common;

namespace PortalApi.Service
{
    public class MskUserIdHandler : IRequestHandler<MskUserIdRequest, MskUserId>
    {
        private readonly IDatabase _database;

        public MskUserIdHandler(IDatabase database)
        {
            _database = database;
        }
        public MskUserId Handle(MskUserIdRequest request)
        {
            return _database.FirstOrDefault<MskUserId>("WHERE " + request.IdType + "=@0", request.Id);
        }
    }
}
