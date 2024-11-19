using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class AJMFZDetails
    {
        public class AJMFZRequest
        {
            public string PassportNumber { get; set; }
        }
        public class AJMFZResponse
        {
            public string LicenseNumber { get; set; }
            public string ShareholderFirstName { get; set; }
            public string ShareholderLastName { get; set; }
            public string ShareHolderNationality { get; set; }
            public string CompanyNameEN { get; set; }
            public string CompanyNameAR { get; set; }
            public string CompanyStatus { get; set; }
            public string LicenseStartDate { get; set; }
            public string LicenseExpiryDate { get; set; }
            public string ShareHolderPercentage { get; set; }
            public string ShareHolderNoOfShares { get; set; }
            public string ResidenceVisaQuotaUsed { get; set; }
        }
    }
}