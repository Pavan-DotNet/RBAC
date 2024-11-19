using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class DLDDetails
    {
        public class Credentials
        {
            public string UserName { get; set; }
            public string Password { get; set; }
        }

        public class searchOwnerRequest
        {
            public Credentials Credentials { get; set; }
            public string OwnerNameEn { get; set; }
            public string OwnerNameAr { get; set; }
            public string OwnerNo { get; set; }
            public string OwnerFamilyNameEn { get; set; }
            public string OwnerFamilyNameAr { get; set; }
            public string OwnerMobile { get; set; }
            public string OwnerCountryID { get; set; }
            public string OwnerCountryNameEn { get; set; }
            public string OwnerCountryNameAr { get; set; }
            public string OwnerPassportNo { get; set; }
            public string OwnerEmirateId { get; set; }
        }

        public class searchOwnerResponse
        {
            public string id { get; set; }
            public string CountryNameAr { get; set; }
            public string CountryNameEn { get; set; }
            public string DBAction { get; set; }
            public string EmiratesId { get; set; }
            public string FamilyNameAr { get; set; }
            public string FamilyNameEn { get; set; }
            public string Identity { get; set; }
            public string MobileNumber { get; set; }
            public string NameAr { get; set; }
            public string NameEn { get; set; }
            public string OwnerNo { get; set; }
            public string PassportNo { get; set; }
        }

        public class searchOwnerPropertiesRequest
        {
            public Credentials Credentials { get; set; }
            public string OwnerNo { get; set; }
        }

        public class searchOwnerPropertiesResponse
        {
            public string id { get; set; }
            public string ActualArea { get; set; }
            public string Address { get; set; }
            public string ApprovalRefNo { get; set; }
            public string AreaId { get; set; }
            public string AreaNameAr { get; set; }
            public string AreaNameEn { get; set; }
            public string BuildingNameAr { get; set; }
            public string BuildingNameEn { get; set; }
            public string ContractAmount { get; set; }
            public string DBAction { get; set; }
            public string Dmno { get; set; }
            public string Dmsubno { get; set; }
            public string Identity { get; set; }
            public string IsFreehold { get; set; }
            public string Landno { get; set; }
            public string Landsubno { get; set; }
            public string PropertyName { get; set; }
            public string PropertyTypeId { get; set; }
            public string ZoneNameAr { get; set; }
            public string ZoneNameEn { get; set; }
        }

        public class getOwnerContractDetails
        {
            public string Land_Number { get; set; }
            public string Land_DM_Number { get; set; }
            public string Land_Area_Id { get; set; }
            public string Land_Area_Name_En { get; set; }
            public string Land_Area_Name_Ar { get; set; }
            public string Building_Name_Ar { get; set; }
            public string Building_Name_En { get; set; }
            public string Contract_Number { get; set; }
            public string Contract_Start_Date { get; set; }
            public string Contract_End_Date { get; set; }
            public string Contract_Amount { get; set; }
            public string Dewa_Premise { get; set; }
            public string Ejari_Sub_Type_Id { get; set; }
            public string Ejari_Sub_Type_Name_Ar { get; set; }
            public string Ejari_Sub_Type_Name_En { get; set; }
            public string Ejari_Type_Id { get; set; }
            public string Ejari_Type_Name_Ar { get; set; }
            public string Ejari_Type_Name_En { get; set; }
            public string Ejari_Property_Type_Id { get; set; }
            public string Property_Number { get; set; }
        }

    }
}
