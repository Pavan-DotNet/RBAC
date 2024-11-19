using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SSSFPDetails
    {
        public class SSSFPRequestParams
        {
            public string EmiratesID { get; set; }
        }
        public class SSSFPResponseParams
        {
            public string PensionerEmiratesID { get; set; }
            public string PensionerNameAR { get; set; }
            public string PensionerNameEN { get; set; }
            public string PensionStartDate { get; set; }
            public string PensionAmount { get; set; }
            public string PensionerDeath { get; set; }

        }

    }
}