using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class ADSSADetails
    {
        public class ADSSADetailsRequest
        {
            public string EmiratesId { get; set; }
        }       

    }
    public class Result
    {
        public string HoHEmiratesId { get; set; }
        public string HoHFullName { get; set; }
        public string BeneficiaryDOB { get; set; }
        public string Benefitstartdate { get; set; }
        public string FinancialSupportStatus { get; set; }
        public double SSASupportAmount { get; set; }
    }

    public class Root
    {
        public Result Result { get; set; }
    }
}