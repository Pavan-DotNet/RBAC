using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class AJMShareDetails
    {
        public class AJMRequestParams
        {
            public string EmiratesId { get; set; }
        }
        public class AJMShareDetailsModel
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            public class Root
            {
                public string CompanyArabicName { get; set; }
                public string CompanyName { get; set; }
                public string CompanyStatus { get; set; }
                public string LicenseExpiryDate { get; set; }
                public string LicenseNumber { get; set; }
                public string LicenseStateDate { get; set; }
                public string Message { get; set; }
                public int ResidenceVisaQuotaUsed { get; set; }
                public string ShareholderFirstName { get; set; }
                public string ShareholderLastName { get; set; }
                public string ShareholderNationality { get; set; }
                public int ShareholderNoOfShares { get; set; }
                public int ShareholderPercentage { get; set; }
            }


        }
    }
}