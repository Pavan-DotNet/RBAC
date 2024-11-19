using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOCDstringegrations.Models
{
    public class PPDetails
    {
        internal string qualificationarDesc;
        internal string qualificationenDesc;


        public string MemberID { get; set; }
        public string AssoicationID { get; set; }
        public string NationalID { get; set; }
        public string unifiedNumber { get; set; }
        public string identityCardnumber { get; set; }
        public string identityBacknumber { get; set; }

        public string identityCardissuDate { get; set; }
        public string identityCardexpiryDate { get; set; }
        public string nationalityid { get; set; }
        public string nationalityarDesc { get; set; }
        public string nationalityenDesc { get; set; }
        public string fullArabicName { get; set; }
        public string firstNameArabic { get; set; }
        public string secondNameArabic { get; set; }
        public string thirdNameArabic { get; set; }
        public string fourthNameArabic { get; set; }
        public string fullEnglishName { get; set; }
        public string firstNameEnglish { get; set; }
        public string secondNameEnglish { get; set; }
        public string thirdNameEnglish { get; set; }
        public string fourthNameEnglish { get; set; }
        public string tribeid { get; set; }
        public string tribearDesc { get; set; }
        public string tribeenDesc { get; set; }
        public string khulasitQaidNumber { get; set; }
        public string familyBookNumber { get; set; }
        public string edbarahNumber { get; set; }
        public string genderid { get; set; }
        public string genderarDesc { get; set; }
        public string genderenDesc { get; set; }
        public string dateOfBirth { get; set; }
        public string countryOfBirthid { get; set; }
        public string countryOfBirtharDesc { get; set; }
        public string countryOfBirthenDesc { get; set; }
        public string emirateOfBirthid { get; set; }
        public string emirateOfBirtharDesc { get; set; }
        public string emirateOfBirthenDesc { get; set; }
        public string cityOfBirthid { get; set; }
        public string cityOfBirtharDesc { get; set; }
        public string cityOfBirthenDesc { get; set; }
        public string placeOfBirthAr { get; set; }
        public string placeOfBirthEn { get; set; }
        public string maritalStatusid { get; set; }
        public string maritalStatusarDesc { get; set; }
        public string maritalStatusenDesc { get; set; }
        public string religionid { get; set; }
        public string religionarDesc { get; set; }
        public string religionenDesc { get; set; }
        public string passportnumber { get; set; }
        public string passporttypeid { get; set; }
        public string passporttypearDesc { get; set; }
        public string passporttypeenDesc { get; set; }
        public string passportissuDate { get; set; }
        public string passportexpiryDate { get; set; }
        public string passportissuePlace { get; set; }
        public string passportissueCountryid { get; set; }
        public string passportissueCountryarDesc { get; set; }
        public string passportissueCountryenDesc { get; set; }
        public string motherNameAr { get; set; }
        public string motherNameEn { get; set; }
        public string localAddressemirateid { get; set; }
        public string localAddressemiratearDesc { get; set; }
        public string localAddressemirateenDesc { get; set; }
        public string localAddresscityid { get; set; }
        public string localAddresscityarDesc { get; set; }
        public string localAddresscityenDesc { get; set; }
        public string localAddressareaid { get; set; }
        public string localAddressareaarDesc { get; set; }
        public string localAddressareaenDesc { get; set; }
        public string localAddressstreetid { get; set; }
        public string localAddressstreetarDesc { get; set; }
        public string localAddressstreetenDesc { get; set; }
        public string building { get; set; }
        public string pobox { get; set; }
        public string mobileNumber { get; set; }

        public string mobileNumber2 { get; set; }
        public string homePhone { get; set; }
        public string workPhone { get; set; }
        public string isInsideCountry { get; set; }
        public string familyCount { get; set; }
        public string familyMaleCount { get; set; }
        public string familyFemaleCount { get; set; }
        public string Email { get; set; }
        public string specialization { get; set; }
        public string qualification { get; set; }
        public string specializationarDesc { get; set; }
        public string specializationenDesc { get; set; }
        public string occupationID { get; set; }
        public string occupationArDesc { get; set; }
        public string occupationEnDesc { get; set; }
    }

    public class ICAModelDetails
    {// Root myDeserializedClass = JsonConvert.Deserializestring<Root>(myJsonResponse);
     // Root myDeserializedClass = JsonConvert.Deserializestring<Root>(myJsonResponse);
        public class Address
        {
            public LocalAddress localAddress { get; set; }
            public string abroadAddress { get; set; }
        }

        public class Area
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class BirthCity
        {
            public string id { get; set; }

            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class BirthCountry
        {
            public string id { get; set; }

            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class BirthEmirate
        {
            public string id { get; set; }

            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class City
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class Department
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class Emirate
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class FileType
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class Gender
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class IdentityCard
        {
            public string IDN { get; set; }
            public string IDNBackNumber { get; set; }
            public string issueDate { get; set; }
            public bool issueDateSpecified { get; set; }
            public string expiryDate { get; set; }
            public bool expiryDateSpecified { get; set; }
        }

        public class ImmigrationFile
        {
            public Status status { get; set; }
            public string fileNo { get; set; }
            public Department department { get; set; }
            public string year { get; set; }
            public FileType fileType { get; set; }
            public string fileSequence { get; set; }
            public string issueDate { get; set; }
            public bool issueDateSpecified { get; set; }
            public string expiryDate { get; set; }
            public bool expiryDateSpecified { get; set; }
        }

        public class IssueCountry
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class LocalAddress
        {
            public Emirate emirate { get; set; }
            public City city { get; set; }
            public Area area { get; set; }
            public Street street { get; set; }
            public string buildingNumber { get; set; }
            public string building { get; set; }
            public string poboxNo { get; set; }
            public string mobileNo { get; set; }
            public string secondMobileNo { get; set; }
            public string homePhone { get; set; }
            public string workPhone { get; set; }
            public string fax { get; set; }
            public string emailAddress { get; set; }
        }

        public class MaritalStatus
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class Nationality
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class Occupation
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class Passport
        {
            public string passportNo { get; set; }
            public PassportType passportType { get; set; }
            public string issueDate { get; set; }
            public bool issueDateSpecified { get; set; }
            public string expiryDate { get; set; }
            public bool expiryDateSpecified { get; set; }
            public IssueCountry issueCountry { get; set; }
            public string issuePlace { get; set; }
        }

        public class PassportType
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class PersonName
        {
            public string firstNameArabic { get; set; }
            public string firstNameEnglish { get; set; }
            public string secondNameArabic { get; set; }
            public string secondNameEnglish { get; set; }
            public string thirdNameArabic { get; set; }
            public string thirdNameEnglish { get; set; }
            public string fourthNameArabic { get; set; }
            public string fourthNameEnglish { get; set; }
            public string fullNameArabic { get; set; }
            public string fullNameEnglish { get; set; }
            public string familyNameArabic { get; set; }
            public string familyNameEnglish { get; set; }
            public string clanNameArabic { get; set; }
            public string clanNameEnglish { get; set; }
            public Tribe tribe { get; set; }
        }

        public class PersonType
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class Qualification
        {
            public string country { get; set; }
            public string academyName { get; set; }
            public Specialization specialization { get; set; }
            public string grade { get; set; }
            public bool qualificationDateSpecified { get; set; }
            public string note { get; set; }
        }

        public class Religion
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class Root
        {
            public string UN { get; set; }
            public IdentityCard identityCard { get; set; }
            public string cardSerialNo { get; set; }
            public Nationality nationality { get; set; }
            public string title { get; set; }
            public PersonName personName { get; set; }
            public string khulasitQaidNo { get; set; }
            public string familyBookNo { get; set; }
            public bool familyBookStartDateSpecified { get; set; }
            public bool familyBookCreateDateSpecified { get; set; }
            public bool citizenshipStartDateSpecified { get; set; }
            public string familyBookRelation { get; set; }
            public string edbarahNo { get; set; }
            public Gender gender { get; set; }
            public string birthDate { get; set; }
            public bool birthDateSpecified { get; set; }
            public BirthCountry birthCountry { get; set; }
            public BirthEmirate birthEmirate { get; set; }
            public BirthCity birthCity { get; set; }
            public string birthPlaceArabic { get; set; }
            public string birthPlaceEnglish { get; set; }
            public string birthCertificateNo { get; set; }
            public MaritalStatus maritalStatus { get; set; }
            public Religion religion { get; set; }
            public string pimarylangCode { get; set; }
            public Passport passport { get; set; }
            public Sponsor sponsor { get; set; }
            public string faith { get; set; }
            public PersonType personType { get; set; }
            public string personClassification { get; set; }
            public ImmigrationFile immigrationFile { get; set; }
            public Occupation occupation { get; set; }
            public string companyTypeCode { get; set; }
            public string companyCode { get; set; }
            public string companyAr { get; set; }
            public string companyEn { get; set; }
            public string motherNameArabic { get; set; }
            public string motherNameEnglish { get; set; }
            public string motherFirstNameArabic { get; set; }
            public string motherFirstNameEnglish { get; set; }
            public string previousNationality { get; set; }
            public List<Address> addresses { get; set; }
            public Qualification qualification { get; set; }
            public string wives_count { get; set; }
            public string wives { get; set; }
            public string relatives { get; set; }
            public bool isBlackListedSpecified { get; set; }
            public bool isInsideCountrySpecified { get; set; }
            public string familyCount { get; set; }
            public string familyMaleCount { get; set; }
            public string familyFemaleCount { get; set; }
            public string motherChildCount { get; set; }
            public string motherChildren { get; set; }
            public bool disabilitySpecified { get; set; }
            public string freeEducation { get; set; }
            public string freeHealth { get; set; }
            public string bloodType { get; set; }
            public string socialSupportStatus { get; set; }
            public string hasSpecialNationality { get; set; }
            public bool lastExitDateSpecified { get; set; }
            public bool lastEntryDateSpecified { get; set; }
            public string icaDenied { get; set; }
            public string fatherHasOnlySon { get; set; }
            public string motherHasOnlySon { get; set; }
            public string signature { get; set; }
            public string portrait { get; set; }
            public string pr_portrait { get; set; }
        }

        public class Specialization
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class Sponsor
        {
            public string nameArabic { get; set; }
            public string nameEnglish { get; set; }
            public Department department { get; set; }
            public string sponsorIDN { get; set; }
            public string sponsorNo { get; set; }
            public bool sponsorNoSpecified { get; set; }
            public List<Address> addresses { get; set; }
            public string sponsorType { get; set; }
        }

        public class Status
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class Street
        {
            public string id { get; set; }
            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }

        public class Tribe
        {
            public string id { get; set; }

            public bool idSpecified { get; set; }
            public string descriptionArabic { get; set; }
            public string descriptionEnglish { get; set; }
        }




    }
}
