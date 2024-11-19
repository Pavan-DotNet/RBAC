using MOCDIntegrations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MOCDIntegrations.Models.GPSSADetails;

namespace MOCDIntegrations.Models
{
    public class GPSSADetails
    {

        public class GPSSARequestParams
        {
            public String EmiratesID
            {
                get;
                set;
            }           
        }

        public class GPSSAPensionService
        {

            public class data
            {
                public string name { get; set; }
                public string customerSegment { get; set; }
                public string customerType { get; set; }
                public string nationality { get; set; }
                public string nonResident { get; set; }
                public string emiratesId { get; set; }
                public string gender { get; set; }
                public string dataOfBirth { get; set; }
                public string familyBookId { get; set; }
                public string familyCityId { get; set; }
                public string familyId { get; set; }
                public string pensionStatus { get; set; }
                public string monthlyPension { get; set; }
                public string pensionStartDate { get; set; }
                public string stopPensionReason { get; set; }
                public string lastDisbursementDate { get; set; }

            }

            public class GPSSAPensionResponse
            {
                public string succeeded { get; set; }
                public List<data> data { get; set; }
                public List<errors> errors { get; set; }
            }

        }


        public class errors
        {
            public string code { get; set; }
            public string Message { get; set; }
        }

        public class GPSSAJobService
        {

            public class data
            {
                public string name { get; set; }
                public string customerSegment { get; set; }
                public string[] customerType { get; set; }
                public string CustomerType { get; set; }
                public string nationality { get; set; }
                public string emiratesId { get; set; }
                public string gender { get; set; }
                public string dataOfBirth { get; set; }
                public string familyBookId { get; set; }
                public string familyCityId { get; set; }
                public string familyId { get; set; }
                public activeEmployment activeEmployment { get; set; }
                public lastEmployment lastEmployment { get; set; }
                public TotalServicePeriod totalServicePeriod { get; set; }

            }

            public class activeEmployment
            {
                public string employerName { get; set; }
                public string startDate { get; set; }
                public string jobTitle { get; set; }
                public string salary { get; set; }

            }

            public class lastEmployment
            {
                public string employerName { get; set; }
                public string startDate { get; set; }
                public string endDate { get; set; }
                public string jobTitle { get; set; }
                public string salary { get; set; }
                public string eosBenefits { get; set; }
            }
            public class TotalServicePeriod
            {
                public string days { get; set; }
                public string months { get; set; }
                public string years { get; set; }
            }
            public class GPSSAJobResponse
            {
                public string succeeded { get; set; }
                public List<data> data { get; set; }
                public List<errors> errors { get; set; }
            }

        }


        public class persona
        {
            public string type { get; set; }
            public string activeStatus { get; set; }

        }
        public class personalDetails
        {
            public IList<persona> persona { get; set; }
            public string fullNameEnglish { get; set; }
            public string fullNameArabic { get; set; }
            public string emiratesId { get; set; }
            public string memberEmirate { get; set; }

        }
        public class previousEmploymentServicePeriodDetails
        {
            public string totalMergedPurchasedPeriodYears { get; set; }
            public string totalMergedPurchasedPeriodMonths { get; set; }
            public string totalMergedPurchasedPeriodDays { get; set; }
            public string totalServicePeriodEosYears { get; set; }
            public string totalServicePeriodEosMonths { get; set; }
            public string totalServicePeriodEosDays { get; set; }

        }
        public class eosBenefitsDetails
        {
            public string eosRemunerationAmount { get; set; }

        }
        public class previousGPSSAEmploymentDetails
        {
            public string employerNameEnglish { get; set; }
            public string employerNameArabic { get; set; }
            public string employerCode { get; set; }
            public string employerSector { get; set; }
            public string employerType { get; set; }
            public string employmentStartDate { get; set; }
            public string employmentEndDate { get; set; }
            public string jobTitle { get; set; }
            public string eosReason { get; set; }
            public string eosSubReason { get; set; }
            public previousEmploymentServicePeriodDetails previousEmploymentServicePeriodDetails { get; set; }
            public eosBenefitsDetails eosBenefitsDetails { get; set; }

        }
        public class retirementPensionDetails
        {
            public string pensionerId { get; set; }
            public string pensionScheme { get; set; }
            public string pensionType { get; set; }
            public string pensionStartDate { get; set; }
            public string grossPensionSalaryAmount { get; set; }
            public string netPensionSalaryAmount { get; set; }
            public string pensionStatus { get; set; }

        }
        public class beneficiaryDeductionDetails
        {
            public string monthlyDeductionAmount { get; set; }

        }
        public class beneficiaryPensionDetails
        {
            public string applicablePensionLawOfBenefactor { get; set; }
            public string benefactorEmiratesId { get; set; }
            public string relationshipWithBenefactor { get; set; }
            public string pensionerId { get; set; }
            public string pensionScheme { get; set; }
            public string pensionType { get; set; }
            public string pensionStartDate { get; set; }
            public string grossPensionSalaryAmount { get; set; }
            public string netPensionSalaryAmount { get; set; }
            public string pensionStatus { get; set; }
            public IList<beneficiaryDeductionDetails> beneficiaryDeductionDetails { get; set; }

        }
        public class gpssaPensionDetails
        {
            public IList<retirementPensionDetails> retirementPensionDetails { get; set; }
            public IList<beneficiaryPensionDetails> beneficiaryPensionDetails { get; set; }

        }

        public class currentEmploymentServicePeriodDetails
        {
            public string mergeServicePeriodYears { get; set; }
            public string mergeServicePeriodMonths { get; set; }
            public string mergeServicePeriodDays { get; set; }
            public string purchaseServicePeriodYears { get; set; }
            public string purchaseServicePeriodMonths { get; set; }
            public string purchaseServicePeriodDays { get; set; }
            public string currentemploymentServicePeriodYears { get; set; }
            public string currentemploymentServicePeriodMonths { get; set; }
            public string currentemploymentServicePeriodDays { get; set; }
            public string totalServicePeriodYears { get; set; }
            public string totalServicePeriodMonths { get; set; }
            public string totalServicePeriodDays { get; set; }

        }
        public class currentGPSSAEmploymentDetails
        {
            public string employerNameEnglish { get; set; }
            public string employerNameArabic { get; set; }
            public string employerCode { get; set; }
            public string employerSector { get; set; }
            public string employerType { get; set; }
            public string employmentStartDate { get; set; }
            public string licenseNumber { get; set; }
            public string gpssaJoinDate { get; set; }
            public currentEmploymentServicePeriodDetails currentEmploymentServicePeriodDetails { get; set; }

        }
        public class insuredContributionDetails
        {
            public string employeeContributiontoGpssa { get; set; }
            public string employerContributiontoGpssa { get; set; }
            public string contributableSalary { get; set; }
            public string lastContributionDate { get; set; }
            public string contributionStartDate { get; set; }

        }

        public class GPSSAPensionAndJob
        {
            public personalDetails personalDetails { get; set; }
            public IList<previousGPSSAEmploymentDetails> previousGPSSAEmploymentDetails { get; set; }
            public IList<gpssaPensionDetails> gpssaPensionDetails { get; set; }
            public IList<currentGPSSAEmploymentDetails> currentGPSSAEmploymentDetails { get; set; }
            public insuredContributionDetails insuredContributionDetails { get; set; }
            public DateTime lastUpdatedTimeStamp { get; set; }

        }

    }
}