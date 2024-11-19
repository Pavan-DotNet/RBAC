using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models.StudentInfo
{
    public class BasicDetails
    {
        public string Id { get; set; }
        public string EmiratesId { get; set; }
        public string EmiratesIdExpiry { get; set; }
        public string BirthDate { get; set; }
        public string BirthCountryEn { get; set; }
        public string GenderEn { get; set; }
        public string GenderAr { get; set; }
        public string NationalityEn { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string FirstLanguage { get; set; }
        public string SpecialStudentFlag { get; set; }
        public PersonName PersonName { get; set; }
    }
}