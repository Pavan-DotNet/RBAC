using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SEWAAccountDetails

    {

        public class Roots
        {
         
            public int Id { get; set; }
            public string inputIdNumber { get; set; }

            public string AccountStatus { get; set; }
            public string BillingCycle { get; set; }
            public string ResponseCode { get; set; }
            public string EmiratesId { get; set; }
            public string IsUAENational { get; set; }
            public string ReceivingInflationAllowance { get; set; }
            public string ResponseMessage { get; set; }
            public string AccountType { get; set; }
            public DateTime InsertDate { get; set; } = DateTime.Now;

        }
    }

}