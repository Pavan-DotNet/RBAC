namespace MOCDIntegrations.Models
{
    public class ADMILITARYDetails
    {
        public class ADMilitaryPensionRequestParams
        {
            public string EmiratesID { get; set; }
        }

        public class ADMilitaryPensionResponseParams
        {
            public string PensionerId { get; set; }
            public string BeneficiaryId { get; set; }
            public string FullNameEnglish { get; set; }
            public string FullNameArabic { get; set; }
            public string AliveHeirFlag { get; set; }
            public string BeneficiaryRelation { get; set; }
            public string NationalIdentifier { get; set; }
            public string PersonType { get; set; }
            public string MonthlyPension { get; set; }
            public string FamilyNumber { get; set; }
            public string TownNumber { get; set; }
            public string DOB { get; set; }
            public string PensionStartDate { get; set; }
            public int EntityId { get; set; }
            public string BeneficiaryName { get; set; }
            public string BeneficiaryStartDate { get; set; }
            public string Status { get; set; }
            public string GeneratedDate { get; set; }
            public int GeneratedBy { get; set; }
        }
    }
}