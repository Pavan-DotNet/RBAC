using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class FamilyBookModel
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class City
        {
            public string id { get; set; }
            public string arDesc { get; set; }
            public string enDesc { get; set; }
        }

        public class CityOfBirth
        {
            public string id { get; set; }
            public string arDesc { get; set; }
            public string enDesc { get; set; }
        }

        public class CountryOfBirth
        {
            public string id { get; set; }
            public string arDesc { get; set; }
            public string enDesc { get; set; }
        }

        public class Dependent
        {
            public string identityCardNumber { get; set; }
            public string unifiedNumber { get; set; }
            public string fullArabicName { get; set; }
            public string fullEnglishName { get; set; }
            public string clanNameArabic { get; set; }
            public string clanNameEnglish { get; set; }
            public Gender gender { get; set; }
            public string motherNameArabic { get; set; }
            public string motherNameEnglish { get; set; }
            public DateTime dateOfBirth { get; set; }
            public CountryOfBirth countryOfBirth { get; set; }
            public EmirateOfBirth emirateOfBirth { get; set; }
            public CityOfBirth cityOfBirth { get; set; }
            public string placeOfBirthAr { get; set; }
            public string placeOfBirthEn { get; set; }
            public MaritalStatus maritalStatus { get; set; }
            public Religion religion { get; set; }
            public Nationality nationality { get; set; }
            public bool activeFlag { get; set; }
            public RelationshipToFamily relationshipToFamily { get; set; }
            public string motherIdentityCardNumber { get; set; }
        }

        public class Dependents
        {
            public List<Dependent> dependent { get; set; }
        }

        public class EmirateOfBirth
        {
            public string id { get; set; }
            public string arDesc { get; set; }
            public string enDesc { get; set; }
        }

        public class FamilyBookDetailsResponse
        {
            public string familySequence { get; set; }
            public City city { get; set; }
            public int childrenCount { get; set; }
            public int wivesCount { get; set; }
            public FamilyHead familyHead { get; set; }
            public Dependents dependents { get; set; }
            public Wives wives { get; set; }
        }

        public class FamilyHead
        {
            public string identityCardNumber { get; set; }
            public string unifiedNumber { get; set; }
            public string fullArabicName { get; set; }
            public string fullEnglishName { get; set; }
            public string clanNameArabic { get; set; }
            public string clanNameEnglish { get; set; }
            public Gender gender { get; set; }
            public string motherNameArabic { get; set; }
            public string motherNameEnglish { get; set; }

            public DateTime dateOfBirth { get; set; }
            public CountryOfBirth countryOfBirth { get; set; }
            public EmirateOfBirth emirateOfBirth { get; set; }
            public CityOfBirth cityOfBirth { get; set; }
            public string placeOfBirthAr { get; set; }
            public string placeOfBirthEn { get; set; }
            public MaritalStatus maritalStatus { get; set; }
            public Religion religion { get; set; }
            public Nationality nationality { get; set; }
            public bool activeFlag { get; set; }
        }

        public class Gender
        {
            public string id { get; set; }
            public string arDesc { get; set; }
            public string enDesc { get; set; }
        }

        public class MaritalStatus
        {
            public string id { get; set; }
            public string arDesc { get; set; }
            public string enDesc { get; set; }
        }

        public class Nationality
        {
            public string id { get; set; }
            public string arDesc { get; set; }
            public string enDesc { get; set; }
        }

        public class RelationshipToFamily
        {
            public string id { get; set; }
            public string arDesc { get; set; }
            public string enDesc { get; set; }
        }

        public class Religion
        {
            public string id { get; set; }
            public string arDesc { get; set; }
            public string enDesc { get; set; }
        }

        public class Root
        {
            public FamilyBookDetailsResponse familyBookDetailsResponse { get; set; }
        }

        public class Wife
        {
            public string identityCardNumber { get; set; }
            public string unifiedNumber { get; set; }
            public string fullArabicName { get; set; }
            public string fullEnglishName { get; set; }
            public string clanNameArabic { get; set; }
            public string clanNameEnglish { get; set; }
            public Gender gender { get; set; }
            public string motherNameArabic { get; set; }
            public DateTime dateOfBirth { get; set; }
            public CountryOfBirth countryOfBirth { get; set; }
            public EmirateOfBirth emirateOfBirth { get; set; }
            public CityOfBirth cityOfBirth { get; set; }
            public string placeOfBirthAr { get; set; }
            public string placeOfBirthEn { get; set; }
            public MaritalStatus maritalStatus { get; set; }
            public Religion religion { get; set; }
            public Nationality nationality { get; set; }
            public bool activeFlag { get; set; }
            public DateTime marriageDate { get; set; }
            public bool marriageDateSpecified { get; set; }
        }

        public class Wives
        {
            public List<Wife> wife { get; set; }
        }


    }
}