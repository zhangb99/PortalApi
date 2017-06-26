using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortalApi.Common
{
    public class ValidicApp
    {
        public string download_link { get; set; }
        public string logo_url { get; set; }
        public string name { get; set; }
        public string excerpt { get; set; }
        public string sync_url { get; set; }
        public bool synced { get; set; }
        public string unsync_url { get; set; }
    }
}
