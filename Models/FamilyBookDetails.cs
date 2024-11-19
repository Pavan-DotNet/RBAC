using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class FamilyBookDetails
    {
        public class FamilyBookDetailsRequest
        {
            public string EmiratesId { get; set; }
        }

        public class FamilyBookDetailsRespose
        {
            public string familySequence { get; set; }=string.Empty;
            public string childrenCount { get; set; }
            public string wivesCount { get; set; }
            public string city { get; set; }
            public string fullArabicName { get; set; }
            public string clanNameArabic { get; set; }
            public string fullEnglishName { get; set; }
            public string clanNameEnglish { get; set; }
            public string unifiedNumber { get; set; }
            public string identityCardNumber { get; set; }
            public string nationality { get; set; }
            public string gender { get; set; }
            public string dateOfBirth { get; set; }
            public string countryOfBirth { get; set; }
            public string placeOfBirthAr { get; set; }
            public string placeOfBirthEn { get; set; }
            public string maritalStatus { get; set; }
            public string religion { get; set; }
            public string motherNameArabic { get; set; }
            public string motherNameEnglish { get; set; }

            public List<Dependent> lstDependent { get; set; }

            public List<Wives> lstWives { get; set; }
        }

        public class Dependent
        {
            public string identityCardNumber { get; set; }
            public string unifiedNumber { get; set; }
            public string familySequence { get; set; }=string.Empty;

            public string city { get; set; }
            public string fullArabicName { get; set; }
            public string clanNameArabic { get; set; }
            public string gender { get; set; }
            public string maritalStatus { get; set; }
            public string motherNameArabic { get; set; }
            public string dateOfBirth { get; set; }
            public string relationshipToFamily { get; set; }
            public string motherIdentityCardNumber { get; set; }
            public string birthCity { get; set; }

        }

        public class Wives
        {
            public string identityCardNumber { get; set; }
            public string unifiedNumber { get; set; }
            public string familySequence { get; set; } = string.Empty;

            public string city { get; set; }
            public string fullArabicName { get; set; }
            public string clanNameArabic;
            public string gender { get; set; }
            public string maritalStatus { get; set; }
            public string motherNameArabic { get; set; }
            public string dateOfBirth { get; set; }
            public string marriageDateSpecified { get; set; }
            public string birthCity { get; set; }
        }
    }
}