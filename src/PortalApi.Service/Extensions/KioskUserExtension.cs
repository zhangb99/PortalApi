using System.Collections.Generic;
using System.Linq;
using PortalApi.Common;

namespace PortalApi.Service
{
    public static class KioskUserExtension
    {
        public static KioskIdentityResponse GetValidKioskUser(this List<KioskPatient> patients, string location)
        {
            var response = new KioskIdentityResponse();
            if (patients == null || patients.Count == 0)
            {
                response.Status = ResponseStatus.NotFound;
                response.Message = "Patient Not Found";
            }
            else
            {
                patients.ForEach(x =>
                {
                    x.ApptOnSite =
                        x.Appointments.Count(y => y.CenterAbbr.ToLower() == location.ToLower()) > 0;
                });

                if (patients.Count > 1)
                {
                    patients.RemoveAll(x => !x.ApptOnSite);
                }

                if (patients.Count == 0)
                {
                    response.Status = ResponseStatus.NotFound;
                    response.Message = "Patient Not Found";
                }
                else if (patients.Count > 1)
                {
                    response.Status = ResponseStatus.Unknown;
                    response.Message = "Multiple patients Found";
                }
                else
                {
                    response.Patient = patients.First();
                    response.Status = ResponseStatus.Success;

                    if (!response.Patient.ApptOnSite)
                    {
                        response.Status = ResponseStatus.Warning;
                        response.Message = "No appointment at current location";
                    }
                }
            }
            return response;
        }
    }
}
