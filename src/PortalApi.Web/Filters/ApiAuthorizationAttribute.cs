using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PortalApi.Web
{
    [AttributeUsage(AttributeTargets.All)]
    public class ApiAuthorizationAttribute : Attribute
    {
        public string AllowClients { get; set; }
        public string DenyClients { get; set; }
        public string AllowUsers { get; set; }
        public string DenyUsers { get; set; }
    }
}