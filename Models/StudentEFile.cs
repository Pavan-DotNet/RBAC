using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class StudentEFile
    {
        public class StudentEFileRequest
        {
            public string EmiratesId { get; set; }
        }
        public class StudentEFileResponse
        {
            public string Area { get; set; }
            public string DisabledCardNo { get; set; }
            public string NationalId { get; set; }
            public string ChildName { get; set; }
            public string Email { get; set; }
            public string WorkPlace { get; set; }
            public string PhoneNumber { get; set; }
            public string MobileNumber { get; set; }
            public string Address { get; set; }
            public string MedicalDiagnosis { get; set; }
            public string HomePhoneNumber { get; set; }
            public string WorkPhoneNumber { get; set; }
            public string FatherMobileNumber { get; set; }
            public string MotherMobileNumber { get; set; }
            public string OtherContactNumbers { get; set; }
            public string GuardianName { get; set; }
            public string DateOfBirth { get; set; }
            public string JoiningDate { get; set; }
            public string RegisterationDate { get; set;}

            public string CountryNameAr { get; set; }
            public string GenderNameAr { get; set; }
            public string StatusTitleAr { get; set; }
            public string MStatusAr { get; set; }
            public string TranReferenceNo { get; set; }
            public string TranRequestDate { get; set; }
            public string RNameAr { get; set; }
            public string TypeTitleAr { get; set; }
            public string EmirateTitleAr { get; set; }
            public string CenterTitleAr { get; set; }
        }
    }
}