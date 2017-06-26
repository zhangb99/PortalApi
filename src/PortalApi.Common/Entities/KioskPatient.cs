using System.Collections.Generic;
using NPoco;

namespace PortalApi.Common
{
    [PrimaryKey("PatId")]
    public class KioskPatient : Patient
    {
        public KioskPatient()
        {
            Appointments = new List<Appointment>();
        }

      
        public bool ApptOnSite { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}
