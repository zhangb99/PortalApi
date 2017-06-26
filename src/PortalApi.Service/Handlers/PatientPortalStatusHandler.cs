using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NPoco;
using PortalApi.Common;
using System.Text.RegularExpressions;

namespace PortalApi.Service
{
    public class PatientPortalAccessHandler : IRequestHandler<PatientPortalAccessRequest, Dictionary<string, bool>>
    {
        private readonly IDatabase _database;

        public PatientPortalAccessHandler(IDatabase database)
        {
            _database = database;
        }
        public Dictionary<string, bool> Handle(PatientPortalAccessRequest request)
        {
            var dic = new Dictionary<string, bool>();
            request.Ids.Split(',').ToList().Distinct().ToList().ForEach(x => dic.Add(x.Trim(), false));

            var idtype = Regex.IsMatch(dic.First().Key, @"\d{8,8}") ? "mrn" : "patid";

            var users = _database.Fetch<string>(@"SELECT DISTINCT " + idtype +
@" FROM [dbo].[UserProxyView] [a] LEFT JOIN [dbo].[ProxyAccessCode] b ON [a].[AccessLevel]=[b].[Value]
WHERE [AccountStatusId] IN ('val','act') AND [PatId] IS NOT NULL AND [AccessLevel]&@0>0 AND [ReminderTypePref]&@1>0",
            request.AccessFlag, request.ReminderFlag);

            users.ForEach(x => { if (dic.ContainsKey(x)) dic[x] = true; });

            return dic;
        }
    }
}
