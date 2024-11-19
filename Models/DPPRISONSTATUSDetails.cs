using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class DPPRISONSTATUSDetails
    {

        public class DPRequest
        {
            public string EmiratesId { get; set; }
        }

        public class DPResponse
        {
            public string EmirateID { get; set; }
            public string PersonNumber { get; set; }
            public string PersonName { get; set; }
            public string InPrisonFlag { get; set; }
        }
    }
}