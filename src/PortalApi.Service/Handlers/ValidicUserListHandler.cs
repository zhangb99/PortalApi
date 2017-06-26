using System.Collections.Generic;
using System.Linq;
using MediatR;
using NPoco;
using PortalApi.Common;
using Flurl.Http;

namespace PortalApi.Service
{
    public class ValidicUserListHandler : IRequestHandler<ValidicUserListRequest, List<ValidicUser>>
    {
        private readonly IDatabase _database;

        public ValidicUserListHandler(IDatabase database)
        {
            _database = database;
        }
        public List<ValidicUser> Handle(ValidicUserListRequest request)
        {
            var url = $"{App.Configuration.ValidicAppBase}/organizations/{App.Configuration.ValidicMskOrgId}/users.json?access_token={App.Configuration.ValidicMskAccessToken}&status=all";

            List<ValidicUser> validiciusers = url.GetJsonAsync<result>().Result.users;

            var portalusers = _database.Fetch<ValidicUser>
            (@"SELECT [a].*, CASE WHEN [x].[PatId] IS NULL THEN 0 ELSE 1 END AS IsPortalUser
FROM [dbo].[ValidicUser] a LEFT JOIN (SELECT DISTINCT [PatId] FROM [dbo].[UserProxyView] WHERE [AccountStatusId] IN ('val', 'act')) x
ON [x].[PatId] = [a].[PatId]");

            var result = from p in portalusers
                         from v in validiciusers
                         .Where(v => v.uid == p.uid).DefaultIfEmpty()
                         select new { p, v };

            return result.Select(x => new ValidicUser
            {
                access_token = x.p.access_token,
                CreatedDate = x.p.CreatedDate,
                IsPortalUser = x.p.IsPortalUser,
                IsProvisioned = x.v == null ? false : true,
                Mrn = x.p.Mrn,
                PatId = x.p.PatId,
                uid = x.p.uid,
                _id = x.p._id
            }).ToList();
        }

        private class result
        {
            public object Summary { get; set; }
            public List<ValidicUser> users { get; set; }
        }
    }
}
