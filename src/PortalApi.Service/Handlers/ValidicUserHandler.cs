using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NPoco;
using PortalApi.Common;
using Flurl.Http;

namespace PortalApi.Service
{
    public class ValidicUserHandler : IRequestHandler<ValidicUserRequest, ValidicUser>
    {
        private readonly IDatabase _database;

        public ValidicUserHandler(IDatabase database)
        {
            _database = database;
        }
        public ValidicUser Handle(ValidicUserRequest request)
        {
            var user = _database.FirstOrDefault<ValidicUser>("WHERE Uid=@0 OR PatId=@0 OR Mrn=@0 OR UID=@0 OR _ID=@0", request.Id);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var url = $"{App.Configuration.ValidicAppBase}organizations/{App.Configuration.ValidicMskOrgId}/apps.json?authentication_token={user.access_token}&access_token={App.Configuration.ValidicMskAccessToken}";

            List<ValidicApp> apps = url.GetJsonAsync<result>().Result.apps;

            if (apps.Any())
            {
                user.Apps = apps;
            }

            return user;
        }

        private class result
        {
            public object Summary { get; set; }
            public List<ValidicApp> apps { get; set; }
        }
    }
}
