using System;
using System.ComponentModel;

namespace PortalApi.Common
{
    /// <summary>
    /// Service/Audit property mapping.  All property marked blob will map into AuditDetail
    /// </summary>
    public enum AuditFieldEnum
    {
        PatId,
        Mrn,
        MsgId,
        ToId,
        FromId,
        WorkstationInfo,
        Blob,
        ClientId,
        ResourceOwner
    }

    [Flags]
    public enum AuditGroupEnum
    {
        [Description("Message")] msg = 1,
        [Description("Patient")] pat = 2,
        [Description("MSKInfo")] msk = 4,
    }
}
