using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class AJMDEDDetails
    {

        public class AJMDEDDetailsRequest
        {
            public string EmiratesId { get; set; }
        }
        public class AJMDEDDetailsResponse
        {
            public string EmiratesID { get; set; }
            public string PassportNumber { get; set; }
            public string NameEN { get; set; }
            public string NameAR { get; set; }
            public string Gender { get; set; }
            public string DateOfBirth { get; set; }
            public string Nationality { get; set; }
            public string PhoneNumber { get; set; }
            public string MobileNumber { get; set; }
            public string BanningReason { get; set; }
            public string BanningStatus { get; set; }
            public string StakeHolderId { get; set; }
            public List<Licenses> Licenses { get; set; }
            public List<LicenseOwnershipDetails> lstLicenseOwnershipDetails { get; set; }
            public List<LicenseActivityDetails> lstLicenseActivityDetails { get; set; }
            public List<LicenseOwnerDetails> lstLicenseOwnerDetails { get; set; }
        }
        public class Licenses
        {
            public string LicenseId { get; set; }
            public string LicenseNumber { get; set; }
        }
        
        public class LicenseOwnershipDetails
        {
            public string LicenseNumber { get; set; }
            public string ApplicationNumber { get; set; }
            public string LicenseNameAR { get; set; }
            public string LicenseNameEN { get; set; }
            public string LicenseTypeAR { get; set; }
            public string LicenseTypeEN { get; set; }
            public string LegalFormAR { get; set; }
            public string LegalFormEN { get; set; }
            public string LicenseStatusAR { get; set; }
            public string LicenseStatusEN { get; set; }
            public string LicenseStartDate { get; set; }
            public string LicenseEndDate { get; set; }
            public string POBoxNo { get; set; }
            public string TelephoneNumber { get; set; }
            public string MobileNumber { get; set; }
            public string EmailAddress { get; set; }
            public string BanningStatus { get; set; }
            public string BanningReason { get; set; }
            public string LicenseClassification { get; set; }
            public string isTaziz { get; set; }
            public string LicenseID { get; set; }
        }

        public class LicenseActivityDetails
        {
            public string SerialNo { get; set; }
            public string LicenseActivityCode { get; set; }
            public string LicenseActivityDescAR { get; set; }
            public string LicenseActivityDescEN { get; set; }

        }

        public class LicenseOwnerDetails
        {
            public string OwnerEmiratesID { get; set; }
            public string OwnerPassportNumber { get; set; }
            public string OwnerNameAR { get; set; }
            public string OwnerNameEN { get; set; }
            public string OwnerGenderAR { get; set; }
            public string OwnerGenderEN { get; set; }
            public string OwnerNationalityAR { get; set; }
            public string OwnerNationalityEN { get; set; }
            public string OwnerPhoneNumber { get; set; }
            public string OwnerMobileNumber { get; set; }
            public string OwnerTypeAR { get; set; }
            public string OwnerTypeEN { get; set; }
            public string LeasorTypeAR { get; set; }
            public string LeasorTypeEN { get; set; }
            public string Notes { get; set; }
            public string LicenseSource { get; set; }
            public string SharePercent { get; set; }
            public string OwnerSerialID { get; set; }
        }

    }



}