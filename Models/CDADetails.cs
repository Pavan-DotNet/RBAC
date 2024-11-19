using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class CDADetails
    {
        public class CDADetailsRequestParams
        {
            public string FamilyNumber { get; set; }
            public string UnifiedNumber { get; set; }
            public string EmiratesId { get; set; }
            public string PassportNo { get; set; }
        }

        public class CDADetailsResponseParams
        {
            public string Aid_Source { get; set; }
            public string Approval_Date { get; set; }
            public string Approved { get; set; }
            public string Beneficiary_Name { get; set; }
            public string Category { get; set; }
            public string CreatedDate { get; set; }
            public string DateofBirth { get; set; }
            public string DateofDeath { get; set; }
            public string Description { get; set; }
            public string Disbusrsment_Status { get; set; }
            public string Education_Level { get; set; }
            public string Error_Message_EmiratesId { get; set; }
            public string Error_Message_Family_No { get; set; }
            public string Error_Message_PH_SR { get; set; }
            public string Error_Message_POD_SR { get; set; }
            public string Error_Message_Passport_No { get; set; }
            public string Error_Message_RFA_SR { get; set; }
            public string Error_Message_TH_spcSR { get; set; }
            public string Error_Message_Unified_No { get; set; }
            public string File_Number { get; set; }
            public string First_Name { get; set; }
            public string Full_Name_Arabic { get; set; }
            public string Good_Manner_and_Behavior_Letter { get; set; }
            public string Health_Situation { get; set; }
            public string Job { get; set; }
            public string Makani_Number { get; set; }
            public string Marital_Status { get; set; }
            public string Monthly_Income { get; set; }
            public string Mother_Name_Arabic { get; set; }
            public string Nationality { get; set; }
            public string New { get; set; }
            public string Notes { get; set; }
            public string Number_of_Family_Members { get; set; }
            public string PHApproved { get; set; }
            public string PHNew { get; set; }
            public string PHNotes { get; set; }
            public string PHSR_pnd { get; set; }
            public string PHStage { get; set; }
            public string PHStatus { get; set; }
            public string PH_ReStudy { get; set; }
            public string POD_Approved { get; set; }
            public string POD_Created_Date { get; set; }
            public string POD_New { get; set; }
            public string POD_Notes { get; set; }
            public string POD_ReStudy { get; set; }
            public string POD_SR_pnd { get; set; }
            public string POD_Stage { get; set; }
            public string POD_Status { get; set; }
            public string POD_Total_Benefit_Approved { get; set; }
            public string Person_is_Dead { get; set; }
            public string PreparingHouseBenefit { get; set; }
            public string Primary_Phone_Number { get; set; }
            public string Re_Study { get; set; }
            public string Relation_Type { get; set; }
            public string Request_Status { get; set; }
            public string SR_pnd { get; set; }
            public string Stage { get; set; }
            public string Status { get; set; }
            public string TH_Approved { get; set; }
            public string TH_Created_Date { get; set; }
            public string TH_New { get; set; }
            public string TH_Notes { get; set; }
            public string TH_ReStudy { get; set; }
            public string TH_SR_pnd { get; set; }
            public string TH_Stage { get; set; }
            public string TH_Status { get; set; }
            public string TH_Total_Benefit_Approved { get; set; }
            public string Total_Benefit_Approved { get; set; }
            public string Type { get; set; }
            public string TypeOfBenefit { get; set; }
            public string TypeOfBenefit_1 { get; set; }
            public string TypeOfBenefit_2 { get; set; }
            public string TypeOfBenefit_3 { get; set; }
            public string Type_of_Housing { get; set; }
            public string Type_of_Income { get; set; }
            public string Zone { get; set; }

        }
    }
}