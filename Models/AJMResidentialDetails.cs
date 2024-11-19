using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class AJMResidentialDetails
    {
        public class AJMResidentialDetailsRequestParams
        {
            public string EmiratesId { get; set; }
        }
        public class AJMResidentialDetail
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            public class Res1
            {
                public bool ResidentialGranted { get; set; }
                public string SitePlanIssuedDate { get; set; }
                public int ParcelID { get; set; }
                public string TypeOfLand { get; set; }
            }

            public class Root
            {
                public List<Res1> Res1 { get; set; }
            }


        }
    }
}