using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class UMMDEDDetails
    {
        public class UMMDEDDetailsRequestParams
        {
            public string EmiratesId { get; set; }
        }

        public class UMMDEDDetailsResponseParams
        {
            public string BusinessLicenseID { get; set; }
            public string BusinessNameAR { get; set; }
            public string BusinessNameEN { get; set; }
            public string EstablishmentDate { get; set; }
            public string ExpiryDate { get; set; }
            public string BusinessLicenseTypeArb { get; set; }
            public List<OwnerDetails> OwnerDetails { get; set; }
            public List<BusinessActivity> BusinessActivity { get; set; }

        }

        public class OwnerDetails
        {
            public string OwnerEmirateID { get; set; }
            public string OwnerFulNameAR { get; set; }
            public string OwnerFulNameEN { get; set; }
            public string OwnerRoleAR { get; set; }
            public string OwnerRoleEN { get; set; }
            public string OwnershipPercentage { get; set; }
        }

        public class BusinessActivity
        {
            public string BusinessActivityDescEN { get; set; }  
            public string BusinessActivityDescAR { get; set; }

        }
    }
}