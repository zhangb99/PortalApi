using System;

namespace PortalApi.Common
{
    public class Patient
    {
        public string PatId { get; set; }
        public string Mrn { get; set; }
        public string DisplayName
        {
            get { return LastName + ", " + FirstName + " " + MiddleName ?? ""; }
        }

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public DateTime Dob { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string HomePhone { get; set; }
        public bool IsPortalUser { get; set; }

        private string _vitalstatus;
        public string VitalStatus
        {
            get
            {
                return string.IsNullOrWhiteSpace(_vitalstatus) ? "Alive" : _vitalstatus;

            }
            set
            {
                _vitalstatus = value;
            }
        }
        public string PrimaryLanguage { get; set; }
    }
}
