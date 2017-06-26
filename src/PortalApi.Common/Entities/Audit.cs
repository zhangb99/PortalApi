using System;
using System.Reflection;
using NPoco;

namespace PortalApi.Common
{
    [TableName("ApiAudit")]
    public class Audit
    {
        public Audit()
        {
            CreatedDate = DateTime.Now;
        }

        public Audit(object request)
        {
            ServiceRequest = request.GetType().Name;
            AuditRequestAttribute ara = (AuditRequestAttribute)Attribute.GetCustomAttribute(request.GetType(), typeof(AuditRequestAttribute));
            
            if (ara.Description != "")
                AuditDetail = ara.Description + " ";

            Type audittype = this.GetType();

            foreach (PropertyInfo property in request.GetType().GetProperties())
            {
                if (!(property.PropertyType.IsPrimitive || property.PropertyType.IsValueType || property.PropertyType == typeof(string)))
                {
                    this.AppendPropertyFrom(property.GetValue(request, null));
                }

                AuditPropertyAttribute attribute = (AuditPropertyAttribute)Attribute
                    .GetCustomAttribute(property, typeof(AuditPropertyAttribute));

                if (attribute != null)
                {
                    PropertyInfo auditproperty = audittype.GetProperty(attribute.AuditKey.ToString());

                    if (auditproperty != null) // custom attribute with a match key
                    {
                        auditproperty.SetValue(this, property.GetValue(request, null), null);
                    }
                    else
                    {
                        if (property.GetValue(request, null) != null)
                        {
                            if (attribute.FormatString != "")
                                AuditDetail += property.Name + ": "
                                    + string.Format(attribute.FormatString, property.GetValue(request, null)) + "|";
                            else
                                AuditDetail += property.Name + ": "
                                    + Convert.ToString(property.GetValue(request, null)) + "|";
                        }
                    }
                }
            }

            ResponseStatus = "Success";
            AuditDetail = AuditDetail.TrimEnd('|');
            CreatedDate = DateTime.Now;
        }

        public int Id { get; set; }

        public string PatId { get; set; }

        public string Mrn { get; set; }

        [Column("MsgToId")]
        public string ToId { get; set; }

        [Column("MsgFromId")]
        public string FromId { get; set; }

        public string AuditGroup { get; set; }

        public string ServiceRequest { get; set; }

        public string WorkstationInfo { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public string AuditDetail { get; set; }
        public string ResponseId { get; set; }

        public string ResponseStatus { get; set; }

        public string ClientId { get; set; }

        public string ResourceOwner { get; set; }
    }
}
