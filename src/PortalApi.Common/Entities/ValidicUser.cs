using System;
using System.Collections.Generic;
using NPoco;

namespace PortalApi.Common
{
    [PrimaryKey("Uid", AutoIncrement = false)]
    public class ValidicUser
    {
        /// <summary>
        /// Portal UUID
        /// </summary>
        public string uid { get; set; }

        /// <summary>
        /// Validic user id
        /// </summary>
        public string _id { get; set; }

        /// <summary>
        /// Validic user access token
        /// </summary>
        public string access_token { get; set; }

        public string PatId { get; set; }

        public string Mrn { get; set; }

        public DateTime CreatedDate { get; set; }

        [ResultColumn]
        public bool IsPortalUser { get; set; }

        /// <summary>
        /// Portal provisioned patient with matching Validic record.  User can be deprovisoned outside portal
        /// </summary>
        [ResultColumn]
        public bool IsProvisioned { get; set; }

        /// <summary>
        /// Linked or available Validic devices or apps
        /// </summary>
        [ResultColumn]
        public List<ValidicApp> Apps { get; set; }
    }
}
