using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class EWEAccountDetails
    {

        public class Roooot
        {
            public int Id { get; set; }
            public string EmiratesID { get; set; }
            public string inputIdNumber { get; set; }
            public string AccountStatus { get; set; }
            public string AccountType { get; set; }
            public string UAENational { get; set; }
            public string BillingCycle { get; set; }
            public string ReceivingInflationAllowance { get; set; }
            public string StatusCode { get; set; }
            public string StatusMessage { get; set; }
            public DateTime InsertDate { get; set; } = DateTime.Now;

        }

    }

}