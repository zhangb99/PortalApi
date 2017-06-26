using System;

namespace PortalApi.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AuditRequestAttribute : Attribute
    {
        /// <summary>
        /// Enable Audit on Request
        /// </summary>
        /// <param name="auditGroup"></param>
        /// <param name="description"></param>
        public AuditRequestAttribute(AuditGroupEnum auditGroup, string description)
        {
            AuditGroup = auditGroup;
            Description = description;
        }
        public AuditGroupEnum AuditGroup { get; set; }
        public string Description { get; set; }
    }

    //[AttributeUsage(AttributeTargets.Class|AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
    [AttributeUsage(AttributeTargets.Property)]
    public class AuditPropertyAttribute : Attribute
    {
        /// <summary>
        /// Mapping request property to audit property
        /// </summary>
        /// <param name="auditKey"></param>
        public AuditPropertyAttribute(AuditFieldEnum auditKey)
        {
            AuditKey = auditKey;
            FormatString = "";
        }

        public AuditFieldEnum AuditKey { get; set; }
        public string FormatString { get; set; }
    }
}
