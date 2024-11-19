using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MOHRECompanyDetails
    {

        public class MOHRERequest
        {
            public string EmiratesId { get; set; }
            public string PassportNo { get; set; }
            public string DOB { get; set; }
        }

        public class NationalCompaniesList
        {
            public string CompanyLicenseNumber { get; set; }
            public string LabourOfficeEng { get; set; }
            public string LabourOfficeArb { get; set; }
            public string CompanyNameEnglish { get; set; }
            public string CompanyNameArabic { get; set; }
            public string CompanyCode { get; set; }
            public string LicenseStartDate { get; set; }
            public string LicenseExpiryDate { get; set; }
            public string TotalEmployee { get; set; }
            public string ComStatus { get; set; }

            public List<ActivitiesList> ActivitiesList { get; set; }
        }
        public static string GetComStatusDescription(string ComStatus)
        {
            switch (ComStatus)
            {
                case "0":
                    return "Has restriction";
                case "1":
                    return "Active";
                case "9":
                    return "Cancelled";
                default:
                    return "Unknown status";
            }
        }
        public class ActivitiesList
        {
            public string englishName { get; set; }
            public string arabicName { get; set; }
        }

        public class MOHREResponse
        {
            public string ownerNameArabic { get; set; }
            public string ownerNameEnglish { get; set; }
            public string SuccessFlag { get; set; }
            public string ErrorFlag { get; set; }
            public string ErrorDescEnglish { get; set; }
            public string ErrorDescArabic { get; set; }
            public List<NationalCompaniesList> NationalCompanyList { get; set; }

        }

    }
}