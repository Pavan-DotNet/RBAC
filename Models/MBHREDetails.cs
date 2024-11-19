using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MBHREDetails
    {
        // Root myDeserializedClass = JsonConvert.Deserializestring<Root>(myJsonResponse);
        public class Pagination
        {
            public int page { get; set; }
            public int pageSize { get; set; }
            public int totalPages { get; set; }
            public int totalItems { get; set; }
        }

        public class Excemptions
        {
            public int id { get; set; }
            public int applicantId { get; set; }
            public int applicationNo { get; set; }
            public string createdOn { get; set; }
            public string approvalDescription { get; set; }
            public string exemptionDescription { get; set; }
            public string otherExemptionReason { get; set; }
            public int familyCount { get; set; }
            public string liabilities { get; set; }
            public string salary { get; set; }
            public string remainingLoanAmount { get; set; }
            public int propertyCount { get; set; }
            public string legacy { get; set; }
            public string fieldVisitNotes { get; set; }
            public string socialStudy { get; set; }
            public string techStudy { get; set; }
            public int salaryPercentage { get; set; }
            public int deathPercentage { get; set; }
            public int healthCostPercentage { get; set; }
            public int agePercentage { get; set; }
            public string name { get; set; }
            public string committeeNotes { get; set; }
        }
        public class GovernmentHelp
        {
            public int applicantId { get; set; }
            public string helpName { get; set; }
            public int governmentalHelpAmount { get; set; }
            public string grantorName { get; set; }
            public DateTime governmentalHelpDate { get; set; }
            public string plotArea { get; set; }
            public string governmentalHelpOwner { get; set; }
            public string attachedWife { get; set; }
        }
        public class Properties
        {
            public int applicantId { get; set; }
            public string plotNumber6 { get; set; }
            public string regionName { get; set; }
            public string cityName { get; set; }
            public string plotArea { get; set; }
            public string ownerPlotArea { get; set; }
            public string description { get; set; }
            public string motasare { get; set; }
            public string motasareDate { get; set; }
            public string motasareValue { get; set; }
            public string isGrantedLandCompensate { get; set; }
            public string amountGrantedLandCompensate { get; set; }
            public string comensateDate { get; set; }
            public string notice { get; set; }
        }
        public class Service
        {
            public int applicantId { get; set; }
            public int applicationNo { get; set; }
            public DateTime applicationDate { get; set; }
            public string serviceTypeName { get; set; }
            public string applicationTypeDesc { get; set; }
            public DateTime? applicationLastUpdateDate { get; set; }
            public string applicationStatusName { get; set; }
            public string regionName { get; set; }
            public string fundingForm { get; set; }
            public string fundingAmount { get; set; }
            public string reasonName { get; set; }
            public string socialStudy { get; set; }
            public string researcherOption { get; set; }
            public string summaryDesc { get; set; }
            public string loanBranchRecommendation { get; set; }
            public int? allowedLoanValue { get; set; }
            public int installment { get; set; }
            public double liabWithIstallmentPer { get; set; }
            public DateTime? loanBoardDate { get; set; }
            public string loanResult { get; set; }
            public string serviceBoardFinanceAmount { get; set; }
            public int grantor { get; set; }
            public DateTime? directorsDependencyDate { get; set; }
            public string board { get; set; }
            public DateTime loanStartDate { get; set; }
            public DateTime lastPaymentDate { get; set; }
            public double? balanceDue { get; set; }
            public double? earlyPayment { get; set; }
            public double? epDiscountPersantage { get; set; }
            public double? totalToPay { get; set; }
            public string loanStatus { get; set; }
            public double? remainingAmount { get; set; }
        }

        public class Root
        {
            public List<Excemptions> excemptions { get; set; }
            public List<GovernmentHelp> governmentHelp { get; set; }
            public List<Properties> properties { get; set; }
            public CustomerServices CustomerServices { get; set; }
            public customerInfo customerInfo { get; set; }

            public Pagination pagination { get; set; }
            public bool success { get; set; }
        }
        public class CustomerServices
        {
            public List<Service> services { get; set; }
            //public List<string> facility { get; set; }
        }
        public class customerInfo
        {
            public string name { get; set; }
            public string emiratesId { get; set; }
            public string placeOfBirth { get; set; }
            public int maritalStatus { get; set; }
            public int familyCount { get; set; }
            public string kholasaNo { get; set; }
            public int kholasaIssuePlace { get; set; }
            public int gender { get; set; }
            public int healthStatus { get; set; }
            public string email { get; set; }
            public DateTime dob { get; set; }
            public string familyNo { get; set; }
            public string buildingNo { get; set; }
            public string edbarahNo { get; set; }
            public string currentAddress { get; set; }
            public int emirate { get; set; }
            public int housingType { get; set; }
            public int area { get; set; }
            public string housePhoneNo { get; set; }
            public string mobileNo { get; set; }
            public int personalPoBoxEmirate { get; set; }
            public string personalPoBox { get; set; }
            public string workingPlace { get; set; }
            public string workingHireDate { get; set; }
            public int workingCity { get; set; }
            public string tradingLicense { get; set; }
            public int propertyIncom { get; set; }
        }


    }
}
