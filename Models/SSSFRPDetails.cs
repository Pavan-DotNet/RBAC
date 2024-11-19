using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SSSFRPDetails
    {

        public class SSSFRPRequestParams
        {
            public string EmiratesID { get; set; }
        }

        public class SSSFRPResponseParams
        {
            public string RetirementPensionersEmiratesID { get; set; }
            public string RetirementPensioners_AR { get; set; }
            public string RetirementPensioners_EN { get; set; }
            public string RetirementPensionersStartDate { get; set; }
            public string RetirementPensionersEndDate { get; set; }
        }
    }
}