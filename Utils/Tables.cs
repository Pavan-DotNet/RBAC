using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Utils
{
    public class Tables
    {
        public static class MSADisabledCard
        {
            public static readonly string TableName = "MSADisabledCard";

            public static readonly string TranReferenceNo = "TranReferenceNo";
            public static readonly string TranRequestDate = "TranRequestDate";
            public static readonly string StatusTitleAr = "StatusTitleAr";
            public static readonly string CardType = "CardType";
            public static readonly string CustomerNameAr = "CustomerNameAr";
            public static readonly string CustomerNameEn = "CustomerNameEn";
            public static readonly string DisabledCardId = "DisabledCardId";
            public static readonly string Nationality = "Nationality";
            public static readonly string CountryNameEn = "CountryNameEn";
            public static readonly string CountryNameAr = "CountryNameAr";
            public static readonly string VisaNo = "VisaNo";
            public static readonly string VisaIssueDate = "VisaIssueDate";
            public static readonly string VisaExpiryDate = "VisaExpiryDate";
            public static readonly string AppFirstName = "AppFirstName";
            public static readonly string AppMiddleName = "AppMiddleName";
            public static readonly string AppLastName = "AppLastName";
            public static readonly string AppFamilyName = "AppFamilyName";
            public static readonly string DiagnosisAuthority = "DiagnosisAuthority";
            public static readonly string AuthTitleAr = "AuthTitleAr";
            public static readonly string AuthTitleEn = "AuthTitleEn";
            public static readonly string DiagnosisInfo = "DiagnosisInfo";
            public static readonly string Disabilities = "Disabilities";
            public static readonly string TypeTitleAr = "TypeTitleAr";
            public static readonly string TypeTitleEn = "TypeTitleEn";
            public static readonly string DiagnosisReportIssuedBy = "DiagnosisReportIssuedBy";
            public static readonly string DiaPosition = "DiaPosition";
            public static readonly string DiaReportDate = "DiaReportDate";
            public static readonly string IsSubmit = "IsSubmit";
            public static readonly string DisabledCardNo = "DisabledCardNo";
            public static readonly string ApplicationDate = "ApplicationDate";
            public static readonly string DisabledName = "DisabledName";
            public static readonly string Gender = "Gender";
            public static readonly string GenderTitleEN = "GenderTitleEN";
            public static readonly string GenderTitleAR = "GenderTitleAR";
            public static readonly string DOB = "DOB";
            public static readonly string POB = "POB";
            public static readonly string CityNo = "CityNo";
            public static readonly string FamilyNo = "FamilyNo";
            public static readonly string PassportNo = "PassportNo";
            public static readonly string IssuePlace = "IssuePlace";
            public static readonly string IssueDate = "IssueDate";
            public static readonly string ExpiryDate = "ExpiryDate";
            public static readonly string Emirate = "Emirate";
            public static readonly string EmirateTitleAr = "EmirateTitleAr";
            public static readonly string EmirateTitleEn = "EmirateTitleEn";
            public static readonly string TranId = "TranId";
            public static readonly string Address1 = "Address1";
            public static readonly string Address2 = "Address2";
            public static readonly string PhoneNumber = "PhoneNumber";
            public static readonly string MobileNumber = "MobileNumber";
            public static readonly string POBox = "POBox";
            public static readonly string Email = "Email";
            public static readonly string IsStudent = "IsStudent";
            public static readonly string CenterId = "CenterId";
            public static readonly string CenterTitleAr = "CenterTitleAr";
            public static readonly string CenterTitleEn = "CenterTitleEn";
            public static readonly string HasJob = "HasJob";
            public static readonly string Company = "Company";
            public static readonly string GetSocialAssistance = "GetSocialAssistance";
            public static readonly string RelationshipLevel = "RelationshipLevel";
            public static readonly string Equips = "Equips";
            public static readonly string AppBy = "AppBy";
            public static readonly string AppDes = "AppDes";
            public static readonly string AppGovOrg = "AppGovOrg";
            public static readonly string OrgTitleAr = "OrgTitleAr";
            public static readonly string OrgTitleEn = "OrgTitleEn";
            public static readonly string RequestType = "RequestType";
            public static readonly string CardIssueDate = "CardIssueDate";
            public static readonly string CardExpiryDate = "CardExpiryDate";
            public static readonly string DisabledFirstNameAr = "DisabledFirstNameAr";
            public static readonly string DisabledMiddleNameAr = "DisabledMiddleNameAr";
            public static readonly string DisabledLastNameAr = "DisabledLastNameAr";
            public static readonly string DisabledFamilyNameAr = "DisabledFamilyNameAr";
            public static readonly string NeedSupporter = "NeedSupporter";
            public static readonly string InstitutionId = "InstitutionId";
            public static readonly string DisabilityLevelId = "DisabilityLevelId";
            public static readonly string LevelTitleEn = "LevelTitleEn";
            public static readonly string LevelTitleAr = "LevelTitleAr";
            public static readonly string CanLiveAlone = "CanLiveAlone";
            public static readonly string MakaniNo = "MakaniNo";
            public static readonly string XCoord = "XCoord";
            public static readonly string YCoord = "YCoord";
            public static readonly string UID = "UID";
            public static readonly string SubTypeId = "SubTypeId";
            public static readonly string SubTypeTitleAr = "SubTypeTitleAr";
            public static readonly string SubTypeTitleEn = "SubTypeTitleEn";
            public static readonly string OtherMobileNo = "OtherMobileNo";
            public static readonly string AccommodationTypeId = "AccommodationTypeId";
            public static readonly string AccommodationTypeAr = "AccommodationTypeAr";
            public static readonly string AccommodationTypeEn = "AccommodationTypeEn";
            public static readonly string WorkFieldId = "WorkFieldId";
            public static readonly string WorkFieldNameEn = "WorkFieldNameEn";
            public static readonly string WorkFieldNameAr = "WorkFieldNameAr";
            public static readonly string WorkingStatusId = "WorkingStatusId";
            public static readonly string WorkingStatusNameEn = "WorkingStatusNameEn";
            public static readonly string WorkingStatusNameAr = "WorkingStatusNameAr";
            public static readonly string EduId = "EduId";
            public static readonly string EduNameEn = "EduNameEn";
            public static readonly string EduNameAr = "EduNameAr";
            public static readonly string MaritalStatusId = "MaritalStatusId";
            public static readonly string MStatusEn = "MStatusEn";
            public static readonly string MStatusAr = "MStatusAr";
            public static readonly string DeliveryMethodId = "DeliveryMethodId";
            public static readonly string TokenId = "TokenId";
            public static readonly string CardTypeId = "CardTypeId";
            public static readonly string ParkingEligibility = "ParkingEligibility";
            public static readonly string TertiaryTypeId = "TertiaryTypeId";
            public static readonly string QuaternaryTypeId = "QuaternaryTypeId";
            public static readonly string SupportingEquipmentAr = "SupportingEquipmentAr";
            public static readonly string SupportingEquipmentEn = "SupportingEquipmentEn";
        }
        public static class MSARegisterInRehablitationCenterRequest
        {
            public static readonly string TableName = "MSARegisterInRehablitationCenterRequest";

            public static readonly string RequestId = "RequestId";
            public static readonly string ChildName = "ChildName";
            public static readonly string NationalityId = "NationalityId";
            public static readonly string GenderId = "GenderId";
            public static readonly string MedicalDiagnosis = "MedicalDiagnosis";
            public static readonly string DateOfBirth = "DateOfBirth";
            public static readonly string RegistrationDate = "RegistrationDate";
            public static readonly string JoiningDate = "JoiningDate";
            public static readonly string GuardianName = "GuardianName";
            public static readonly string RelationshipId = "RelationshipId";
            public static readonly string Address = "Address";
            public static readonly string WorkPlace = "WorkPlace";
            public static readonly string PhoneNo = "PhoneNo";
            public static readonly string MobileNo = "MobileNo";
            public static readonly string HomePhoneNo = "HomePhoneNo";
            public static readonly string WorkPhoneNo = "WorkPhoneNo";
            public static readonly string FatherMobileNo = "FatherMobileNo";
            public static readonly string MotherMobileNo = "MotherMobileNo";
            public static readonly string OtherContactNo = "OtherContactNo";
            public static readonly string TranId = "TranId";
            public static readonly string MaritalStatusId = "MaritalStatusId";
            public static readonly string Email = "Email";

            public static readonly string DisabilityTypeId = "DisabilityTypeId";
            public static readonly string EmirateId = "EmirateId";
            public static readonly string Area = "Area";
            public static readonly string NationalId = "NationalId";
            public static readonly string DisabledCardNo = "DisabledCardNo";
            public static readonly string CenterId = "CenterId";

            public static readonly string CountryNameAr = "CountryNameAr";
            public static readonly string GenderNameAr = "GenderNameAr";
            public static readonly string StatusTitleAr = "StatusTitleAr";
            public static readonly string MStatusAr = "MStatusAr";
            public static readonly string TranReferenceNo = "TranReferenceNo";
            public static readonly string TranRequestDate = "TranRequestDate";
            public static readonly string RNameAr = "RNameAr";
            public static readonly string TypeTitleAr = "TypeTitleAr";
            public static readonly string EmirateTitleAr = "EmirateTitleAr";
            public static readonly string CenterTitleAr = "CenterTitleAr";
        }
    }
}