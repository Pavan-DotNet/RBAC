using System.Collections.Generic;

namespace MOCDIntegrations.Models
{
    public class ADJDDetails
    {
        public class ADJDDetailsRequestParams
        {
            public string EmiratesID { get; set; }
            public string Passport { get; set; }
        }

        public class ADJDDetailsResponseParams
        {
            public List<MarriageContractDetails> lstMarriageContractDetails { get; set; }
            public List<PartyDetails> lstPartyDetails { get; set; }
        }

        public class MarriageContractDetails
        {
            public string ReferenceNumber { get; set; }
            public string FolderNumber { get; set; }
            public string FolderYear { get; set; }
            public string ContractNumber { get; set; }
            public string ContractDate { get; set; }
            public string RegionCode { get; set; }
            public string RegionDescriptionArabic { get; set; }
            public string RegionDescriptionEnglish { get; set; }
            public string CourtNameCode { get; set; }
            public string CourtNameDescriptionArabic { get; set; }
            public string CourtNameDescriptionEnglish { get; set; }
            public string CityCode { get; set; }
            public string CityDescriptionArabic { get; set; }
            public string CityDescriptionEnglish { get; set; }
            public string MarriagePlaceCode { get; set; }
            public string MarriagePlaceDescriptionArabic { get; set; }
            public string MarriagePlaceDescriptionEnglish { get; set; }
        }

        public class PartyDetails
        {
            public string FirstName { get; set; }
            public string SecondName { get; set; }
            public string ThirdName { get; set; }
            public string FourthName { get; set; }
            public string FamilyName { get; set; }
            public string NameInEnglish { get; set; }
            public string IDTypeCode { get; set; }
            public string IDTypeDescriptionArabic { get; set; }
            public string IDTypeDescriptionEnglish { get; set; }
            public string EmiratesID { get; set; }
            public string DateOfBirth { get; set; }
            public string GenderCode { get; set; }
            public string GenderDescriptionArabic { get; set; }
            public string GenderDescriptionEnglish { get; set; }
            public string ReligionCode { get; set; }
            public string ReligionDescriptionArabic { get; set; }
            public string ReligionDescriptionEnglish { get; set; }
            public string PartyRoleCode { get; set; }
            public string PartyRoleDescriptionArabic { get; set; }
            public string PartyRoleDescriptionEnglish { get; set; }
            public string EmailAddress { get; set; }
            public string Telephone { get; set; }
            public string PlaceOfBirth { get; set; }
            public string PlaceOfResidence { get; set; }
            public string RegionCode { get; set; }
            public string RegionDescriptionArabic { get; set; }
            public string RegionDescriptionEnglish { get; set; }
            public string CityCode { get; set; }
            public string CityDescriptionArabic { get; set; }
            public string CityDescriptionEnglish { get; set; }
            public string CountryCode { get; set; }
            public string CountryDescriptionArabic { get; set; }
            public string CountryDescriptionEnglish { get; set; }
            public string NationalityCode { get; set; }
            public string NationalityDescriptionArabic { get; set; }
            public string NationalityDescriptionEnglish { get; set; }
            public string QualificationCode { get; set; }
            public string QualificationDescriptionArabic { get; set; }
            public string QualificationDescriptionEnglish { get; set; }
            public string ProfessionDescription { get; set; }
        }
    }
}