using System;
using System.Collections.Generic;
using System.Linq;
using MediatR;
using NPoco;
using PortalApi.Common;

namespace PortalApi.Service
{
    public class PatientHandler : IRequestHandler<PatientInfoRequest, List<Patient>>
    {
        private readonly IDatabase _database;

        public PatientHandler(IDatabase database)
        {
            _database = database;
        }
        public List<Patient> Handle(PatientInfoRequest request)
        {
            var result = _database.Fetch<Patient>(
                "EXEC Patient_SearchAccountWithValidUser @@search=@0, @@PatId=@1, @@MRN=@2",
                request.Name, request.PatId, request.Mrn);

            List<Patient> idbpatient = _database.Fetch<Patient>(
                "EXEC MSKCC.dbo.Admin_queryEWSClarity @@UserName=@0, @@PatId=@1, @@MRN=@2, @@EdgCredential=''",
                request.Name, request.PatId, request.Mrn);

            result.AddRange(idbpatient.Where(i =>
                result.All(p => !string.Equals(p.PatId.Trim(), i.PatId.Trim(), StringComparison.CurrentCultureIgnoreCase))));

            return result;
        }
    }
}
