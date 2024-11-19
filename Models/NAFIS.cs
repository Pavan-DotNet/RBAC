using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class NAFIS
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Data
        {
            public string EmirateID { get; set; }
            public string Name { get; set; }
            public List<FinancialBenefit> FinancialBenefits { get; set; }
            public List<Payment> Payments { get; set; }
            public string HasActiveFinancialBenefits { get; set; }
            public string CreatedOn { get; set; }
        }

        public class FinancialBenefit
        {
            public string Name { get; set; }
            public DateTime FirstPaymentDate { get; set; }
            public double TotalAmount { get; set; }
        }

        public class Payment
        {
            public string Benefit { get; set; }
            public double Amount { get; set; }
            public DateTime ToBeProcessedAfter { get; set; }
            public string Status { get; set; }
            public DateTime PaidOn { get; set; }
        }

        public class NAFISRoot
        {
            public Data Data { get; set; }
            public string Message { get; set; }
            public string ErrorCode { get; set; }
            public bool IsSucceeded { get; set; }
        }


    }
}