using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class FUJLANDDetails
    {
        public class FUJLANDDetailsRequestParams
        {
            public string EmiratesID { get; set; }
        }

        public class LandDetails
        {
            public string EmiratesID { get; set; }
            public string RealEstateOwner { get; set; }
            public string OwnershipPercentage { get; set; }
            public string RealEstateLocation { get; set; }
            public string RealEstateNumber { get; set; }
            public string OwnershipDate { get; set; }
            public string RealEstateType { get; set; }
            public string RealEstateStatus { get; set; }
            public string PlotNumber { get; set; }
            public string BlockNumber { get; set; }
            public string LastRefreshDate { get; set; }
        }
    }
}