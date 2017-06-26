using System.ComponentModel;
using Newtonsoft.Json;

namespace PortalApi.Common
{
    public class MskUserId
    {
        public MskUserId()
        {
            NtId = "";
            SmsDoctorId = "";
            HisId = "";
            PsmId = "";
            Email = "";
            DivisionId = "";
            DepartmentId = "";
        }

        /// <summary>
        /// Employee Id
        /// </summary>
        public string EmpId { get; set; }

        /// <summary>
        /// Active Directory NTID
        /// </summary>
        public string NtId { get; set; }

        /// <summary>
        /// SMS Doctor Id, NOT to be confused with SMSID
        /// </summary>
        [DefaultValue("")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string SmsDoctorId { get; set; }

        /// <summary>
        /// HIS Id
        /// </summary>
        public string HisId { get; set; }

        /// <summary>
        /// Portal Secure Messaging Mailbox Id
        /// </summary>
        public string PsmId { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// Division Id from Connect
        /// </summary>
        public string DivisionId { get; set; }

        /// <summary>
        /// Department Id from Connect
        /// </summary>
        public string DepartmentId { get; set; }

    }
}
