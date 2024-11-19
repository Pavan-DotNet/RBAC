using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class POD
    {
        public class PODCardRequest
        {
            public string EmiratesId { get; set; }
        }
        public class PODCardResponse
        {
            public string TranRequestDate { get; set; }
            public string TranReferenceNo { get; set; }
            public string StatusTitleAr { get; set; }
            public string CustomerNameAr { get; set; }
            public string CustomerNameEn { get; set; }
            public string CanLiveAlone { get; set; }
            public string NeedSupporter { get; set; }
            public bool IsStudent { get; set; }
            public string QualificationAr { get; set; }
            public string QualificationEn { get; set; }
            public string MaritalStatusAr { get; set; }
            public string MaritalStatusEn { get; set; }
            public string AccommodationTypeAr { get; set; }
            public string AccommodationTypeEn { get; set; }
            public string WorkFieldAr { get; set; }
            public string WorkFieldEn { get; set; }
            public string WorkingStatusAr { get; set; }
            public string WorkingStatusEn { get; set; }
            public string DisabilitySubTypeAr { get; set; }
            public string DisabilitySubTypeEn { get; set; }
            public string DisabilityLevelAr { get; set; }
            public string DisabilityLevelEn { get; set; }
            public string CenterNameAr { get; set; }
            public string CenterNameEn { get; set; }
            public string InstitutionNameAr { get; set; }
            public string InstitutionNameEn { get; set; }
            public string DiagnosisAuthorityAr { get; set; }
            public string DiagnosisAuthorityEn { get; set; }
            public string EmirateAr { get; set; }
            public string EmirateEn { get; set; }
            public string GenderAr { get; set; }
            public string GenderEn { get; set; }
            public string NationalityAr { get; set; }
            public string NationalityEn { get; set; }
            public string OtherMobileNo { get; set; }
            public string UID { get; set; }
            public string POBox { get; set; }
            public string MakaniNo { get; set; }
            public string XCoord { get; set; }
            public string YCoord { get; set; }
            public string Company { get; set; }
            public string DisabilityTitleAR { get; set; }
            public string DisabilityTitleEN { get; set; }
            public string Speciality { get; set; }
            public string ReportIssuedBy { get; set; }
            public string SupportingEquipmentAr { get; set; }
            public string SupportingEquipmentEn { get; set; }
            public string DiagnosisInformation { get; set; }
            public string Email { get; set; }
            public string MobileNo { get; set; }
            public string PhoneNo { get; set; }
            public string Address { get; set; }
            public string FamilyNameAR { get; set; }
            public string FatherNameAR { get; set; }
            public string FirstNameAR { get; set; }
            public string GrandfatherNameAR { get; set; }
            public string FamilyNameEN { get; set; }
            public string FatherNameEN { get; set; }
            public string FirstNameEN { get; set; }
            public string GrandfatherNameEN { get; set; }
            public string IdentificationNo { get; set; }
            public string DisabledCardNo { get; set; }
            public string CardType { get; set; }
            //public DateTime ReportDate { get; set; }
            public string DateOfBirth { get; set; }
            public string ResidenceExpiryDate { get; set; }
            public string CardIssueDate { get; set; }
            public string CardExpiryDate { get; set; }
            public string OrganizationAr { get; set; }
            public string OrganizationEn { get; set; }
        }
    }

public class PODDetails
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class GetDisabilityInfoResponse
        {
            public GetDisabilityInfoResult getDisabilityInfoResult { get; set; }
        }

        public class GetDisabilityInfoResult
        {
            private DateTime _cardIssueDate;
            private DateTime _cardExpiryDate;
            private DateTime _dateOfBirth;

            public DateTime CardIssueDate
            {
                get => _cardIssueDate.Date;
                set => _cardIssueDate = value;
            }

            public DateTime CardExpiryDate
            {
                get => _cardExpiryDate.Date;
                set => _cardExpiryDate = value;
            }

            public DateTime DateOfBirth
            {
                get => _dateOfBirth.Date;
                set => _dateOfBirth = value;
            }
            public string ResidenceExpiryDate { get; set; }
            public bool? IsStudent { get; set; }
            public bool? HasJob { get; set; }
            public bool? CanLiveAlone { get; set; }
            public bool? NeedSupporter { get; set; }
            public string CountryCode { get; set; }
            public string EmirateTitleAr { get; set; }
            public string EmirateTitleEn { get; set; }
            public string CountryNameEn { get; set; }
            public string CountryNameAr { get; set; }
            public string GenderTitleAr { get; set; }
            public string GenderTitleEn { get; set; }
            public object UID { get; set; }
            public object YCoord { get; set; }
            public object XCoord { get; set; }
            public object MakaniNo { get; set; }
            public object Company { get; set; }
            public string Email { get; set; }
            public string MobileNo { get; set; }
            public object PhoneNo { get; set; }
            public string Address { get; set; }
            public string FullNameAR { get; set; }
            public string FullNameEN { get; set; }
            public string DisabledCardNo { get; set; }
            public string DisabilityTypeAR { get; set; }
            public string DisabilityTypeEN { get; set; }
            public object SupportingEquipmentAR { get; set; }
            public object SupportingEquipmentEN { get; set; }
            public string IdentificationNo { get; set; }
            public object DisabilityLevelTitleEn { get; set; }
            public object DisabilityLevelTitleAr { get; set; }
            public string DisabilityTypeId { get; set; }
            public string CountryId { get; set; }
            public ResponseStatus ResponseStatus { get; set; }
        }

        public class ResponseStatus
        {
            public string ResponseCode { get; set; }
            public string ResponseTitle { get; set; }
            public string ResponseDescription { get; set; }
        }

        public class Root
        {
            public GetDisabilityInfoResponse getDisabilityInfoResponse { get; set; }
        }


    }
}