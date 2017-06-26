using Westwind.Utilities.Configuration;
using System.Web.Security;
using System.Configuration;
using System;
using System.Text;

namespace PortalApi.Common
{
    public class App
    {
        public static ApplicationConfiguration Configuration { get; set; }

        static App()
        {
            Configuration = new ApplicationConfiguration();

            var provider = new ConfigurationFileConfigurationProvider<ApplicationConfiguration>()
            {
                ConfigurationSection = "appSettings",
                EncryptionKey = Encoding.Default.GetString(MachineKey.Unprotect(
                    Convert.FromBase64String(ConfigurationManager.AppSettings["EncryptionKey"]))),
                PropertiesToEncrypt = "ValidicMskAccessToken"
            };

            Configuration.Initialize(provider);
        }
    }

    public class ApplicationConfiguration : AppConfiguration
    {
        public string Environment { get; set; }
        public string EncryptionKey { get; set; }
        public string SsoAuthority { get; set; }
        public string RequiredScope { get; set; }
        public string ApplicationRoot { get; set; }
        public int ClientConfigCacheMinutes { get; set; }
        public string ValidicMskOrgId { get; set; }
        public string ValidicMskAccessToken { get; set; }
        public string ValidicAppBase { get; set; }
    }
}
