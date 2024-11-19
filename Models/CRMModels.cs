using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public enum EntityCodes
    {
        SRA = 10104,
        EDD = 10112,
        PFM = 10347
    }

    public class MOCDResponse<T>
    {
        public MOCDErroCode Code { get; set; }
        public string ResponseTitle { get; set; }
        public string ResponseDescriptionEn { get; set; }
        public string ResponseDescriptionAr { get; set; }
        public T Content { get; set; }
        public int validationCode { get; set; }
    }

    public enum MOCDErroCode
    {
        Success = 200,
        Error = 500,
        NotFound = 404,
        Exception = 400
    }

    public class GrantRequest
    {
        public Guid Id { get; set; }
        public string HusbandNationalId { get; set; }
        public string WifeNationalId { get; set; }
        public DateTime MarriageContractDate { get; set; }
        public Guid Court { get; set; }
        public Guid EmployerCategory { get; set; }
        public string HusbandFullNameArabic { get; set; }
        public string HusbandFullNameEnglish { get; set; }
        public DateTime? HusbandBirthDate { get; set; }
        public Guid HusbandEducationLevel { get; set; }
        public string HusbandMobile1 { get; set; }
        public string HusbandMobile2 { get; set; }
        public string HusbandEmail { get; set; }
        public string WifeFullNameArabic { get; set; }
        public string WifeFullNameEnglish { get; set; }
        public DateTime? WifeBirthDate { get; set; }
        public Guid WifeEducationLevel { get; set; }
        public string WifeMobile1 { get; set; }
        public string WifeEmail { get; set; }
        public string FamilyBookNumber { get; set; }
        public string TownNumber { get; set; }
        public string FamilyNumber { get; set; }
        public DateTime FamilyBookIssueDate { get; set; }
        public Guid FamilyBookIssuePlace { get; set; }
        public string Employer { get; set; }
        public Guid WorkPlace { get; set; }
        public string HusbandEducationLevelAr { get; set; }
        public string HusbandEducationLevelEn { get; set; }
        public string WifeEducationLevelAr { get; set; }
        public string WifeEducationLevelEn { get; set; }
        public decimal Totalmonthlyincome { get; set; }
        public Guid BankName { get; set; }
        public string IBAN { get; set; }
        public Guid TypeOfRequest { get; set; }
        public Guid RequestingService { get; set; }
        public Guid Status { get; set; }
        public string ArabicStatus { get; set; }
        public string EnglishStatus { get; set; }
        public string ApplicationNo { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string SourceType { get; set; }
        public string PortalType { get; set; }
        public int ChannelType { get; set; }
        public string Channel { get; set; }
        public int PreferredMonth { get; set; }
        public int[] ReasonForNotRecievinggrant { get; set; }
        public bool? HusbandAttendedEdaad { get; set; }
        public bool? WifeAttendedEdaad { get; set; }
        public string ServiceProcess { get; set; }
        public string FormGroup { get; set; }
        public string UserID { get; set; }
    }

    public class PFMembership
    {
        public Guid Id { get; set; }
        public string MembershipNumber { get; set; }
        public string FullNameInArabic { get; set; }
        public string FullName { get; set; }
        public string ArabicName { get; set; }
        public string Name { get; set; }
        public Guid Bank { get; set; }
        public string IBAN { get; set; }
        public int[] MarketingChannels { get; set; }
        public string ReferenceNo { get; set; }
        public string IDNumber { get; set; }
        public Guid Emirate { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string MobileNumber1 { get; set; }
        public string MobileNumber2 { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? Birthdate { get; set; }
        public string Email { get; set; }
        public int Gender { get; set; }
        public int MaritalStatus { get; set; }
        public Guid EducationLevel { get; set; }
        public bool HasWork { get; set; }
        public string Employer { get; set; }
        public int AverageIncome { get; set; }
        public string Description { get; set; }
        public string ArabicDescription { get; set; }
        public bool IsPoD { get; set; }
        public bool IsSocialSecurity { get; set; }
        public int SocialSecurityCategory { get; set; }
        public bool HasHouseLicense { get; set; }
        public string LicenseNumber { get; set; }
        public Guid IssuerEmirate { get; set; }
        public string IssuerAuthoriy { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool HasHealthCard { get; set; }
        public Guid HealthCardIssuer { get; set; }
        public string HealthCardNumber { get; set; }
        public DateTime? HealthCardIssueDate { get; set; }
        public DateTime? HealthCardExpiryDate { get; set; }
        public string Snapchat { get; set; }
        public string Instagram { get; set; }
        public string Twitter { get; set; }
        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public string ProjectBrief { get; set; }
        public bool IsTested { get; set; }
        public DateTime? TestDate { get; set; }
        public string[] ProductCategories { get; set; }
        public int Status { get; set; }
        public string StatusEn { get; set; }
        public string StatusAr { get; set; }
        public string RejectedReason { get; set; }
        public DateTime? SubmissionOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Note { get; set; }
        public Guid TypeOfRequest { get; set; }
        public Guid RequestingService { get; set; }
        public int ChannelType { get; set; }
        public string ServiceProcess { get; set; }
        public string FormGroup { get; set; }
        public string UserId { get; set; }
        public bool AppAccept { get; set; }
        public bool Whatsapp { get; set; }
        public string ServiceCode { get; set; }
    }
    
    public class EdaadInfo
    {
        public Guid Id { get; set; }
        public Guid Husband { get; set; }
        public string HusbandNationalId { get; set; }
        public string HusbandFullNameArabic { get; set; }
        public string HusbandFullNameEnglish { get; set; }
        public string HusbandMobile { get; set; }
        public string HusbandEmail { get; set; }
        public Guid Wife { get; set; }
        public string WifeNationalId { get; set; }
        public string WifeFullNameArabic { get; set; }
        public string WifeFullNameEnglish { get; set; }
        public string WifeMobile { get; set; }
        public string WifeEmail { get; set; }
        public Guid TypeOfRequest { get; set; }
        public Guid RequestingService { get; set; }
        public bool? HusbandAttendedEdaad { get; set; }
        public string HusbandAttendedSessions { get; set; }
        public DateTime? HusbandAttendedDate { get; set; }
        public string HusbandCertificateUrl { get; set; }
        public bool? WifeAttendedEdaad { get; set; }
        public string WifeAttendedSessions { get; set; }
        public DateTime? WifeAttendedDate { get; set; }
        public string WifeCertificateUrl { get; set; }
    }
}