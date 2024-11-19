using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDstringegrations.Models
{
    public class PersonalProfileDetails
    {
        public class PersonalProfileDetailsRequest
        {
            public string EmiratesId { get; set; }

        }

        public class PersonalProfileDetailsResponse
        {
            public string unifiedNumber { get; set; }
            public string identityCardnumber { get; set; }
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
        }
    }

    public class PersonalProfileDetailsModel
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public class Root
        {
            public PersonInquiryResponse personInquiryResponse { get; set; }
        }

        public class PersonInquiryResponse
        {
            public Header Header { get; set; }
            public Body Body { get; set; }
        }

        public class Header
        {
            public string ServiceName { get; set; }
            public string SourceChannel { get; set; }
            public string ServiceVersion { get; set; }
            public string ServiceLanguage { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public string Hash { get; set; }
        }

        public class Body
        {
            public string TransactionRefNo { get; set; }
            public string ResponseCode { get; set; }
            public string ResponseDescription { get; set; }
            public PersonProfile PersonProfile { get; set; }
            public string Timestamp { get; set; }
        }

        public class PersonProfile
        {
            public string UN { get; set; }
            public IdentityCard IdentityCard { get; set; }
            public Nationality Nationality { get; set; }
            public PersonName PersonName { get; set; }
            public Gender Gender { get; set; }
            public string BirthDate { get; set; }
            public BirthCountry BirthCountry { get; set; }
            public BirthEmirate BirthEmirate { get; set; }
            public BirthCity BirthCity { get; set; }

            public string BirthPlaceArabic { get; set; }
            public string BirthPlaceEnglish { get; set; }
            public MaritalStatus MaritalStatus { get; set; }
            public Religion Religion { get; set; }
            public Passport Passport { get; set; }
            public Sponsor Sponsor { get; set; }
            public PersonType PersonType { get; set; }
            public ImmigrationFile ImmigrationFile { get; set; }
            public Occupation Occupation { get; set; }
            public Addresses Addresses { get; set; }
            public Qualification Qualification { get; set; }
            public string FamilyCount { get; set; }
            public string FamilyMaleCount { get; set; }
            public string FamilyFemaleCount { get; set; }

            public string FamilyBookNumber { get; set; }
            public string khulasitQaidNo { get; set; }
        }

        public class IdentityCard
        {
            public string IDN { get; set; }
            public string IDNBackNumber { get; set; }
            public string IssueDate { get; set; }
            public string ExpiryDate { get; set; }
        }

        public class Nationality
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class PersonName
        {
            public string FirstNameArabic { get; set; }
            public string FirstNameEnglish { get; set; }
            public string SecondNameArabic { get; set; }
            public string SecondNameEnglish { get; set; }
            public string ThirdNameArabic { get; set; }
            public string ThirdNameEnglish { get; set; }
            public string FourthNameArabic { get; set; }
            public string FourthNameEnglish { get; set; }
            public string FullNameArabic { get; set; }
            public string FullNameEnglish { get; set; }
            public string FamilyNameArabic { get; set; }
            public string FamilyNameEnglish { get; set; }
            public Tribe Tribe { get; set; }
        }

        public class Tribe
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class Gender
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class BirthCountry
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }
        public class BirthEmirate
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }
        public class BirthCity
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }
        

        public class MaritalStatus
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class Religion
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class Passport
        {
            public string PassportNo { get; set; }
            public PassportType PassportType { get; set; }
            public string IssueDate { get; set; }
            public string ExpiryDate { get; set; }
            public IssueCountry IssueCountry { get; set; }
            public string IssuePlace { get; set; }
        }

        public class PassportType
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class IssueCountry
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class Sponsor
        {
            public string NameArabic { get; set; }
            public string NameEnglish { get; set; }
            public Department Department { get; set; }
            public string SponsorNo { get; set; }
            public Addresses Addresses { get; set; }
        }

        public class Department
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class Addresses
        {
            public List<AddressWrapper> Address { get; set; }
        }

        public class AddressWrapper
        {
            public LocalAddress LocalAddress { get; set; }
        }

        public class LocalAddress
        {
            public Emirate Emirate { get; set; }
            public City City { get; set; }
            public Area Area { get; set; }
            public Street Street { get; set; }
            public string Building { get; set; }
            public string PoboxNo { get; set; }
            public string MobileNo { get; set; }
            public string HomePhone { get; set; }
            public string Fax { get; set; }
            public string EmailAddress { get; set; }
            public string WorkPhone { get; set; }

            
        }

        public class Emirate
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class City
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class Area
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }
        public class Street
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }
        public class PersonType
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class ImmigrationFile
        {
            public Status Status { get; set; }
            public string FileNo { get; set; }
            public Department Department { get; set; }
            public string Year { get; set; }
            public FileType FileType { get; set; }
            public string FileSequence { get; set; }
            public string IssueDate { get; set; }
            public string ExpiryDate { get; set; }
        }

        public class Status
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class FileType
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class Occupation
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }

        public class Qualification
        {
            public Specialization Specialization { get; set; }
        }

        public class Specialization
        {
            public string Id { get; set; }
            public string DescriptionArabic { get; set; }
            public string DescriptionEnglish { get; set; }
        }
    }

}