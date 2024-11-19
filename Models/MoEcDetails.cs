using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MoEcDetails
    {
        public class MoEcRequestParams
        {
            public string EmiratesID { get; set; }
            public string Passport { get; set; }

        }

        public class MoEcResponseParams
        {
            public List<OwnerDetails> lstOwnerDetails { get; set; }
            public List<BusinessLicenses> lstBusinessLicenses { get; set; }
            public List<OwnerPartnership> lstOwnerPartnership { get; set; }

        }

        public class OwnerDetails
        {
            public string OwnerEmiratesID { get; set; }
            public string OwnerNameAR { get; set; }
            public string OwnerNameEN { get; set; }
            public string OwnerPassport { get; set; }
        }
        public class BusinessLicenses
        {
            public string LicenseNumber { get; set; }
            public string BusinessNameAr { get; set; }
            public string BusinessNameEn { get; set; }
            public string EconomicDepartmentAR { get; set; }
            public string EconomicDepartmentEN { get; set; }
            public string EmirateAR { get; set; }
            public string EmirateEN { get; set; }
            public string LicenseRegistrationDate { get; set; }
            public string LicenseExpiryDate { get; set; }
            public string LicenseStatusAR { get; set; }
            public string LicenseStatusEN { get; set; }
            public string LicenseFCRNumber { get; set; }
        }
        public class OwnerPartnership
        {
            public string OwnerPartnershipTypeAR { get; set; }
            public string OwnerPartnershipTypeEN { get; set; }
            public string OwnerSharePercentage { get; set; }
        }

    }
}