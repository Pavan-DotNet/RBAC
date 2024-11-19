using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class FUJDEDDetails
    {

        public class FUJDEDDetailsRequestParams
        {
            public string EmiratesID { get; set; }
        }

        public class LicenseDetails
        {
            public string EmiratesID { get; set; }
            public string LicenseHolderName { get; set; }
            public string LicenseHolderCategory { get; set; }
            public string PartnershipRatio { get; set; }
            public string LicenseIssuanceDate { get; set; }
            public string LicenseExpiryDate { get; set; }
            public string LicenseNumber { get; set; }
            public string LicenseName { get; set; }
            public string LastRefreshDate { get; set; }
        }
    }
}