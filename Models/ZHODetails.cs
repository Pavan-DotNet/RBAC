using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDstringegrations.Models
{

    public class ZHODetails
    {
        public string PODID { get; set; }
        public string EID { get; set; }
        public string FULL_NAME_EN { get; set; }
        public string FULL_NAME_AR { get; set; }
        public string DisabilityAxisCode { get; set; }
        public string DisabilityAxisAr { get; set; }
        public string DisabilityAxisEn { get; set; }
        public string FK_DISABILITY { get; set; }
        public string DISABILITY_AR { get; set; }
        public string DISABILITY_EN { get; set; }
        public string DATE_OF_BIRTH { get; set; }
        public string IsDeath { get; set; }
        public string DATE_OF_DEATH { get; set; }
        public string AgeYears { get; set; }
        public string FAMILY_BOOK_NO { get; set; }
        public string UNIFIED_NO { get; set; }
        public string PASSPORT_NO { get; set; }
        public string PASSPORT_EXPIRY { get; set; }
        public string VISA_EXPIRY { get; set; }
        public string FK_GENDER { get; set; }
        public string GENDER_AR { get; set; }
        public string GENDER_EN { get; set; }
        public string FK_CITIZEN { get; set; }
        public string CITIZEN_Ar { get; set; }
        public string CITIZEN_En { get; set; }
        public string FK_NATIONALITY { get; set; }
        public string NATIONALITY_AR { get; set; }
        public string NATIONALITY_EN { get; set; }
        public string DefaultLang { get; set; }
        public string FK_RELIGION { get; set; }
        public string RELIGION_AR { get; set; }
        public string RELIGION_EN { get; set; }
        public string FK_REGION { get; set; }
        public string REGION_AR { get; set; }
        public string REGION_EN { get; set; }
        public string FK_ACCOMMODATION_TYPE { get; set; }
        public string ACCOMMODATION_TYPE_AR { get; set; }
        public string ACCOMMODATION_TYPE_EN { get; set; }
        public string FK_DISABILITY_LEVEL { get; set; }
        public string DISABILITY_LEVEL_AR { get; set; }
        public string DISABILITY_LEVEL_EN { get; set; }
        public string FK_SUB_DISABILITY { get; set; }
        public string REG_IN_CENTER { get; set; }
        public string REG_IN_SCHOOL { get; set; }
        public string SCHOOL_AR { get; set; }
        public string SCHOOL_EN { get; set; }
        public string EMAIL { get; set; }
        public string MOBILE1 { get; set; }
        public string MOBILE2 { get; set; }
        public string HOME_TEL { get; set; }
        public string MobileNo_FromSEHA { get; set; }
        public string HomeNo_FromSEHA { get; set; }
        public string Email_FromSEHA { get; set; }
        public string FK_MARITAL_STATUS { get; set; }
        public string MARITAL_STATUS_AR { get; set; }
        public string MARITAL_STATUS_EN { get; set; }
        public string FK_QUALIFICATION { get; set; }
        public string QUALIFICATION_AR { get; set; }
        public string QUALIFICATION_EN { get; set; }
        public string IS_WORKING { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string IsConfirmed { get; set; }
        public string HasZHOPODCard { get; set; }
        public string ZHOPODCARDNo { get; set; }
        public string ZHOPODCard_IssueDate { get; set; }
        public string ZHOPODCard_ExpiryDate { get; set; }
        public string HasFazaaCard { get; set; }
        public string FazaaCardNo { get; set; }
        public string IsZHOStudent { get; set; }
        public string ZHOStudentNo { get; set; }
        public string ZHOStudentProgram_CODE { get; set; }
        public string ZHOStudentProgram_AR { get; set; }
        public string ZHOStudentProgram_EN { get; set; }
        public string IsValidEID { get; set; }
        public string Has_Social_Support { get; set; }
        public string SSA_Support_Amount { get; set; }
        public string CENTER_CODE { get; set; }
        public string CENTER_AR { get; set; }
        public string CENTER_EN { get; set; }
        public string FK_SOURCE { get; set; }
        public string CREATED_ON { get; set; }
        public string CREATED_BY { get; set; }
        public string UPDATED_ON { get; set; }
        public string UPDATED_BY { get; set; }
        public bool Status { get; set; }
    }
}
