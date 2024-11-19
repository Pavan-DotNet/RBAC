using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MOCDIntegrations.Models
{
    public class ADRPBFDetails_New
    {
        public class ADRPBFDetailsRequestParams
        {
            public string EmiratesID { get; set; }
            public string TNo { get; set; }
            public string FNo { get; set; }
        }
        public class ResponseCode
        {
            public string H { get; set; }
            public string Text { get; set; }
        }

    }
    public class ADRPBFDetailsEmiratedId
    {


        public class MemberProfileInformation
        {
            public string ActiveStartDate { get; set; }
            public string ActiveemployerNameAr { get; set; }
            public string ActiveemployerNameEn { get; set; }
            public string ActivejobTitle { get; set; }
            public string Activesalary { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string DOB { get; set; }
            public string Email { get; set; }
            public string Emirate { get; set; }
            public string EmployerSector { get; set; }
            public string FamilyBookCityID { get; set; }
            public string FamilyBookNo { get; set; }
            public string Fax { get; set; }
            public string LastemployerName { get; set; }
            public string LastemployerNameEn { get; set; }
            public string LastendDate { get; set; }
            public string LastjobTitle { get; set; }
            public string Lastsalary { get; set; }
            public string LaststartDate { get; set; }
            public string MemberFullNameArabic { get; set; }
            public string MemberFullNameEnglish { get; set; }
            public string MemberType { get; set; }
            public string Mobile { get; set; }
            public string NationalID { get; set; }
            public string POBOX { get; set; }
            public string PensionAmount { get; set; }
            public string PensionLastDate { get; set; }
            public string PensionNumber { get; set; }
            public string PensionStartDate { get; set; }
            public string PensionStatus { get; set; }
            public string Phone { get; set; }
        }
    }

    public class ADRPBFDetailsFamily
    {


        public class MemberInformationResponse
        {
            public string Address { get; set; }
            public string City { get; set; }
            public string DOB { get; set; }
            public string Email { get; set; }
            public string Emirate { get; set; }
            public string FamilyBookNo { get; set; }
            public string Fax { get; set; }
            public string MemberFullNameArabic { get; set; }
            public string MemberFullNameEnglish { get; set; }
            public string MemberType { get; set; }
            public string Mobile { get; set; }
            public string NationalID { get; set; }
            public string POBOX { get; set; }
            public string PensionAmount { get; set; }
            public string PensionNumber { get; set; }
            public string PensionStartDate { get; set; }
            public string Phone { get; set; }
            public string BeneficiaryInformation { get; set; }
        }


    }

}