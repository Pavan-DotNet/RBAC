using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class TAWTHEEQDetails
    {
        public class TawtheeqRequestParam
        {
            public string EmiratesID { get; set; }
        }
        public class TawtheeqDetailsRequest
        {
            public ResponseStatus objResponseStatus { get; set; }
            public string ContractNo { get; set; }
            public string ContractStartDate { get; set; }
            public string ContractExpiryDate { get; set; }
            public string ContractStatusCode { get; set; }
            public string ContractArabicDescription { get; set; }
            public string ContractEnglishDescription { get; set; }

        }
        public class TawtheeqDetailsResponse
        {
            public ResponseStatus objResponseStatus { get; set; }
            public List<ContractDetails> lstContractDetails { get; set; }
            public List<TenantDetails> lstTenantDetails { get; set; }
            public List<OwnersDetails> lstOwnersDetails { get; set; }
            public List<LessorDetails> lstLessorDetails { get; set; }
            public List<RentAndPaymentsDetails> lstRentAndPaymentsDetails { get; set; }
            public List<PropertyDetails> lstPropertyDetails { get; set; }

            public List<TawtheeqDetailsRequest> listTawtheeqDetailsRequest { get; set; }

         
        }

        public class ResponseStatus
        {
            public string StatusCode { get; set; }
            public string StatusDescriptionEnglish { get; set; }
            public string StatusDescriptionArabic { get; set; }
        }

        public class ContractDetails
        {
            public string ContractNo { get; set; }
            public string ContractRegistrationDate { get; set; }
            public string ContractUsage { get; set; }
            public string Code { get; set; }
            public string ArabicDescription { get; set; }
            public string EnglishDescription { get; set; }
            public string StatusArabicDescription { get; set; }
            public string StatusEnglishDescription { get; set; }

        }
        public class OwnersDetails
        {
            public string PlotOwnerName { get; set; }
        }
        public class LessorDetails
        {
            public string LessorEmiratesID { get; set; }
            public string LessorNameArabic { get; set; }
            public string LessorNameEnglish { get; set; }
            public string ContactPersonName { get; set; }
            public string MobileNumber { get; set; }
            public string eMailAddress { get; set; }
            public string LessorCityCode { get; set; }
            public string LessorCityArabicDescription { get; set; }
            public string LessorCityEnglishDescription { get; set; }
            public string NationalityCode { get; set; }
            public string NationalityArabicDescription { get; set; }
            public string NationalityEnglishDescription { get; set; }
            public string PhoneNumber { get; set; }
            public string Fax { get; set; }
            public string POBox { get; set; }
        }
        public class TenantDetails
        {
            public string TenantEmiratesID { get; set; }
            public string TenantNameArabic { get; set; }
            public string TenantNameEnglish { get; set; }
            public string eMailAddress { get; set; }
            public string MobileNumber { get; set; }
            public string TenantCity { get; set; }
            public string TenantCityCode { get; set; }
            public string TenantCityArabicDescription { get; set; }
            public string TenantCityEnglishDescription { get; set; }
            public string TenantNationality { get; set; }
            public string NationalityCode { get; set; }
            public string NationalityArabicDescription { get; set; }
            public string NationalityEnglishDescription { get; set; }
            public string PhoneNumber { get; set; }
            public string POBox { get; set; }
            public string TradeLicenseNo { get; set; }
            public string IssuePlace { get; set; }
        }
        public class RentAndPaymentsDetails
        {
            public string ContractStartDate { get; set; }
            public string ContractExpiryDate { get; set; }
            public string SecurityDeposit { get; set; }
            public string RentalValue { get; set; }
            public string TenancycontractDuration { get; set; }
            public string NumberofInstallments { get; set; }
            public string ADWEABillPaidBy { get; set; }
        }
        public class PropertyDetails
        {
            public string PropertyRegistrationNumber { get; set; }
            public string PropertyName { get; set; }
            public string PropertyTypeCode { get; set; }
            public string PropertyTypeArabicDescription { get; set; }
            public string PropertyTypeEnglishDescription { get; set; }
            public string ZoneCode { get; set; }
            public string ZoneArabicDescription { get; set; }
            public string ZoneEnglishDescription { get; set; }
            public string SectorCode { get; set; }
            public string SectorArabicDescription { get; set; }
            public string SectorEnglishDescription { get; set; }
            public string PlotNo { get; set; }
            public string StreetRoadCode { get; set; }
            public string StreetRoadArabicDescription { get; set; }
            public string StreetRoadEnglishDescription { get; set; }
            public string PremisesNumber { get; set; }
            public List<UnitDetails> lstUnitDetails { get; set; }

        }

        public class UnitDetails
        {
            public string UnitRegistrationNumber { get; set; }
            public string UnitName { get; set; }
            public string UnitTypeCode { get; set; }
            public string UnitTypeArabicDescription { get; set; }
            public string UnitTypeEnglishDescription { get; set; }
            public string NumberOfBedRooms { get; set; }
            public string OtherRoomsFloorNo { get; set; }
            public string OtherRoomsNoOfOccupants { get; set; }
            public string OtherRoomsLeasedAreaM2 { get; set; }
            public string ActualUsageCode { get; set; }
            public string ActualUsageArabicDescription { get; set; }
            public string ActualUsageEnglishDescription { get; set; }
            public string PremisesNumber { get; set; }
            public string OccupantsDetails { get; set; }
            public string OccupantsDetailsIDNumber { get; set; }
            public string OccupantsDetailsIDType { get; set; }
            public string IDTypeCode { get; set; }
            public string IDTypeArabicDescription { get; set; }
            public string IDTypeEnglishDescription { get; set; }
            public string CompanyEducationalInstituteName { get; set; }
            public string OccupantNameArabic { get; set; }
            public string OccupantNameEnglish { get; set; }
        }


    }
}