using System;
using System.Linq;
using Fasterflect;
using MediatR;
using PortalApi.Common;

namespace PortalApi.Service
{
    public static class AuditHelper
    {
        public static Audit CreateAudit<TResponse>(this IRequest<TResponse> request)
        {
            var apirequest = request as ApiRequest<TResponse>;
            if (apirequest == null) return null;

            var attr = request.GetType().Attribute<AuditRequestAttribute>();
            if (attr == null) return null;

            return new Audit
            {
                ServiceRequest = apirequest.GetType().Name,
                ResponseStatus = "running",
                ClientId = apirequest.ClientId,
                ResourceOwner = apirequest.ResourceOwner,
                WorkstationInfo = apirequest.WorkstationInfo,
                AuditGroup = attr.AuditGroup.ToString(),
                AuditDetail = attr.Description,
            };
        }

        public static Audit AppendPropertyFrom(this Audit audit, object request, object result)
        {
            var requestproperties = request.GetType().Properties()
                .Where(x => Attribute.IsDefined(x, typeof(AuditPropertyAttribute))).ToList();

            var audittype = audit.GetType();

            foreach (var prop in requestproperties)
            {
                var attr = prop.Attributes().FirstOrDefault(x => x is AuditPropertyAttribute) as AuditPropertyAttribute;

                if (attr != null)
                {
                    var auditprop = audittype.GetProperty(attr.AuditKey.ToString());

                    if (auditprop != null)
                    {
                        audit.SetPropertyValue(attr.AuditKey.ToString(),
                            request.GetPropertyValue(attr.AuditKey.ToString()));
                    }
                    else // blob
                    {
                        if (prop.GetValue(request, null) != null)
                        {
                            if (!string.IsNullOrWhiteSpace(attr.FormatString))
                                audit.AuditDetail += " " + prop.Name + ": " +
                                                     string.Format(attr.FormatString,
                                                         prop.GetValue(request, null)) + "|";
                            else
                                audit.AuditDetail += " " + prop.Name + ": "
                                                     + Convert.ToString(prop.GetValue(request, null)) + "|";
                        }
                    }
                }
            }

            audit.AuditDetail = (audit.AuditDetail ?? "").TrimEnd('|');

            var apiResponse = result as ApiResponse;

            if (apiResponse != null)
            {
                var response = apiResponse;
                audit.ResponseStatus = response.Status.ToString();
                audit.ResponseId = response.ResponseId;
                audit.AuditDetail += response.IsCached ? " cached" : "";
            }
            else
            {
                audit.ResponseStatus = "Success";
            }

            return audit;
        }
    }
}