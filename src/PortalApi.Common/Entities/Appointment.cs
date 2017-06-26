using NPoco;

namespace PortalApi.Common
{
    [PrimaryKey("AptPatId")]
    public class Appointment
    {
        [Column("EWSPatId")]
        public string AptPatId { get; set; }

        [Column("Apt_Id")]
        public string CSN { get; set; }

        [Column("Apt_Date")]
        public string ApptDate { get; set; }

        [Column("APT_Time")]
        public string ApptTime { get; set; }

        [Column("APT_Len")]
        public string ApptLength { get; set; }

        [Column("APT_DepartmentId")]
        public string DeptID { get; set; }

        [Column("Address1")]
        public string DeptName { get; set; }

        [Column("ED_DepartmentName_NotForDisplay")]
        public string DeptName4HL7 { get; set; }

        [Column("ED_CenterCat")]
        public string CenterCat { get; set; }

        [Column("ED_CenterAbbr")]
        public string CenterAbbr { get; set; }

        [Column("ED_CenterTitle")]
        public string CenterName { get; set; }

        [Column("APT_ExtProviderId")]
        public string ProviderID { get; set; }

        [Column("APT_ExternalProviderName")]
        public string ProviderName { get; set; }

        [Column("APT_ExtProviderName_NotForDisplay")]
        public string ProviderName4HL7 { get; set; }

        [Column("APT_VisitTypeId")]
        public string VisitTypeID { get; set; }

        [Column("APT_VisitTypeAbbr")]
        public string VisitTypeAbbr { get; set; }

        [Column("APT_ExternalVisitTypeName")]
        public string VisitTypeName { get; set; }

        [Column("APT_VisitTypeName_NotForDisplay")]
        public string VisitTypeName4HL7 { get; set; }

        [Column("APT_Status")]
        public string ApptStatus { get; set; }

        [Column("APT_DisplayCheckin")]
        public string DisplayCheckin { get; set; }

        [Column("APT_AllowCheckin")]
        public string AllowCheckin { get; set; }
    }
}
