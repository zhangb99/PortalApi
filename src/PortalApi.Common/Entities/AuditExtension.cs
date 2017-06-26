using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PortalApi.Common
{
    public static class AuditExtension
    {
        public static Audit AppendPropertyFrom(this Audit audit, object request)
        {
            foreach (PropertyInfo property in request.GetType().GetProperties())
            {
                AuditPropertyAttribute attribute =
                    (AuditPropertyAttribute)Attribute.GetCustomAttribute(property, typeof(AuditPropertyAttribute));

                if (attribute == null) continue;


                PropertyInfo auditproperty = audit.GetType().GetProperty(attribute.AuditKey.ToString());

                if (auditproperty != null) // custom attribute with a match key
                {
                    auditproperty.SetValue(audit, property.GetValue(request, null), null);
                }
                else
                {
                    if (attribute.FormatString != "")
                        audit.AuditDetail += " " + property.Name + ": " +
                                             string.Format(attribute.FormatString,
                                                           property.GetValue(request, null)) + "|";
                    else
                        audit.AuditDetail += " " + property.Name + ": "
                                             + Convert.ToString(property.GetValue(request, null)) + "|";
                }
            }
            return audit;
        }
    }
}
