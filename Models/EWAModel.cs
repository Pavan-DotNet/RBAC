using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class EWAModel
    {
        public class Root
        {
            public long EmiratesID { get; set; }
            public string AccountStatus { get; set; }
            public string AccountType { get; set; }
            public string UAENational { get; set; }
            public string BillingCycle { get; set; }
            public string ReceivingInflationAllowance { get; set; }
            public string StatusCode { get; set; }
            public string StatusMessage { get; set; }
        }

    }
}