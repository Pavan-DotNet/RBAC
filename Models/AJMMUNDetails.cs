using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class AJMMUNDetails
    {
        public class AJMMUNRequest
        {
            public string EmiratesID { get; set; }
        }
        public class AJMMUNResponse
        {
            public string TypeOfLand { get; set; }
            public string ResidentialGranted { get; set; }
            public string ParcelID { get; set; }
            public string SitePlanIssuedDate { get; set; }
        }
    }
}