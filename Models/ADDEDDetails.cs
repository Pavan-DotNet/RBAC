using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Windows.Forms;
using static MOCDIntegrations.Models.ADDEDDetails.TradeLicenseAllData;

namespace MOCDIntegrations.Models
{
    public class ADDEDDetails
    {
        public class BusinessLicenseStatus
        {
            public string BusinessLicenseStatusCode                          { get; set; }
            public string BusinessLicenseStatusArb                          { get; set; }
            public string BusinessLicenseStatusEng                          { get; set; }
        }

        public class EnvBody
        {
            public GetTradeLicensePartnersByEIDResponse GetTradeLicensePartnersByEIDResponse { get; set; }
        }

        public class EnvEnvelope
        {
            [JsonProperty("@xmlns:soap")]
            public string XmlnsSoap { get; set; }

            [JsonProperty("@xmlns:xsd")]
            public string XmlnsXsd { get; set; }

            [JsonProperty("@xmlns:env")]
            public string XmlnsEnv { get; set; }

            [JsonProperty("@xmlns:xsi")]
            public string XmlnsXsi { get; set; }

            [JsonProperty("env:Header")]
            public object EnvHeader { get; set; }

            [JsonProperty("env:Body")]
            public EnvBody EnvBody { get; set; }
        }


        public class GetTradeLicensePartnersByEIDResponse
        {
            [JsonProperty("@xmlns")]
            public string Xmlns { get; set; }
            public GetTradeLicensePartnersByEIDResult GetTradeLicensePartnersByEIDResult { get; set; }
        }

        public class GetTradeLicensePartnersByEIDResult
        {
            public TradeLicenses TradeLicenses { get; set; }
            public string status { get; set; }
        }


        public class Root
        {
            [JsonProperty("?xml")]
            public Xml xml { get; set; }

            [JsonProperty("soap:Envelope")]
            public SoapEnvelope soapEnvelope { get; set; }
        }

        public class SoapBody
        {
            public GetTradeLicensePartnersByEIDResponse GetTradeLicensePartnersByEIDResponse { get; set; }
        }

        public class SoapEnvelope
        {
            [JsonProperty("@xmlns:env")]
            public string xmlnsenv { get; set; }

            [JsonProperty("@xmlns:xsd")]
            public string xmlnsxsd { get; set; }

            [JsonProperty("@xmlns:soap")]
            public string xmlnssoap { get; set; }

            [JsonProperty("@xmlns:xsi")]
            public string xmlnsxsi { get; set; }

            [JsonProperty("soap:Header")]
            public object soapHeader { get; set; }

            [JsonProperty("soap:Body")]
            public SoapBody soapBody { get; set; }
        }
        public class LegalForm
        {
            public string LegalFormCode                  { get; set; }
            public string BusinessLegalFormArb           { get; set; }
            public string BusinessLegalFormEng              { get; set; }
        }

        //public class Root
        //{
        //    [JsonProperty("?xml")]
        //    public Xml Xml { get; set; }

        //    [JsonProperty("env:Envelope")]
        //    public EnvEnvelope EnvEnvelope { get; set; }
        //}

        public class TradeLicenseModel
        {
            public string TradeLicenseNumber            { get; set; }
            public string BusinessNameArb                   { get; set; }
            public string BusinessNameEng                   { get; set; }
            public LegalForm LegalForm { get; set; }
            public BusinessLicenseStatus BusinessLicenseStatus { get; set; }
        }

        public class TradeLicenses
        {
            public List<TradeLicenseModel> TradeLicenseModel { get; set; }
        }

        public class Xml
        {
            [JsonProperty("@version")]
            public string Version { get; set; }

            [JsonProperty("@encoding")]
            public string Encoding { get; set; }
        }

        public class TradeLicenseAllData
        {
            public class Activity
            {
                public List<Activity> activity { get; set; }
                public string ActivityCode              { get; set; }
                public string ActivityNameArb           { get; set; }
                public string ActivityNameEng           { get; set; }
                public string TradeLicenseNumber                { get; set; }

            }
            public class Activity1
            {

                public Activity1 activity1 { get; set; }
                public string ActivityCode              { get; set; }
                public string ActivityNameArb               { get; set; }
                public string ActivityNameEng               { get; set; }
            }

            public class BranchType
            {
                public string BranchTypeCode                { get; set; }
                public string BranchTypeArb             { get; set; }
                public string BranchTypeEng                 { get; set; }
            }

            public class BusinessLicenseStatus
            {
                public string BusinessLicenseStatusCode                 { get; set; }
                public string BusinessLicenseStatusArb          { get; set; }
                public string BusinessLicenseStatusEng              { get; set; }
            }

            public class Clasification
            {
                public string ClasificationCode                 { get; set; }
                public string ClasificationArb          { get; set; }
                public string ClasificationEng          { get; set; }
            }

            public class Root
            {
                [JsonProperty("?xml")]
                public Xml xml { get; set; }

                [JsonProperty("soap:Envelope")]
                public SoapEnvelope soapEnvelope { get; set; }
            }

            public class SoapBody
            {
                public GetTradeLicenseBasicDetailsResponse GetTradeLicenseBasicDetailsResponse { get; set; }
            }
            public class GetTradeLicenseBasicDetailsResponse
            {
                [JsonProperty("@xmlns")]
                public string xmlns { get; set; }
                public GetTradeLicenseBasicDetailsResult GetTradeLicenseBasicDetailsResult { get; set; }
            }

            public class GetTradeLicenseBasicDetailsResult
            {
                public string status { get; set; }


                public Partner partner { get; set; }
                public Partner1 partner1 { get; set; }
                public Master master { get; set; }
                public Master1 master1 { get; set; }
                public Activity activity { get; set; }
                public Activity1 activity1 { get; set; }
            }
            public class SoapEnvelope
            {
                [JsonProperty("@xmlns:env")]
                public string xmlnsenv { get; set; }

                [JsonProperty("@xmlns:xsd")]
                public string xmlnsxsd { get; set; }

                [JsonProperty("@xmlns:soap")]
                public string xmlnssoap { get; set; }

                [JsonProperty("@xmlns:xsi")]
                public string xmlnsxsi { get; set; }

                [JsonProperty("soap:Header")]
                public object soapHeader { get; set; }

                [JsonProperty("soap:Body")]
                public SoapBody soapBody { get; set; }
            }

            public class EstablishmentCountry
            {
                public string CountryCode       { get; set; }
                public string CountryNameArb                { get; set; }
                public string CountryNameEng                    { get; set; }
            }

            public class IssuePlace
            {
                public string IssuePlaceCode                 { get; set; }
                public string IssuePlaceArb                  { get; set; }
                public string IssuePlaceEng                  { get; set; }
            }

            public class LegalForm
            {
                public string LegalFormCode                                 { get; set; }
                public string BusinessLegalFormArb { get; set; }
                public string BusinessLegalFormEng { get; set; }
            }

            public class LicenseRelationship
            {
                public string LicenseRelationshipCode                   { get; set; }
                public string BusinessLicenseRelationshipArb             { get; set; }
                public string BusinessLicenseRelationshipEng                 { get; set; }
            }

            public class LicenseType
            {
                public string LicenseTypeCode               { get; set; }
                public string BusinessLicenseTypeArb                 { get; set; }
                public string BusinessLicenseTypeEng                    { get; set; }
            }

            public class ListTradeLicenseAllDataResponse
            {
                [JsonProperty("@xmlns")]
                public string Xmlns { get; set; }
                public ListTradeLicenseAllDataResult ListTradeLicenseAllDataResult { get; set; }
            }

            public class ListTradeLicenseAllDataResult
            {
                public Partner partner { get; set; }
                public Partner1 partner1 { get; set; }


                public PartnerDto PartnerDto { get; set; }
                public MasterDto MasterDto { get; set; }


                public Master master { get; set; }
                public Master1 master1 { get; set; }

                public Activity activity { get; set; }
                public Activity1 activity1 { get; set; }

                public string status { get; set; }
            }

            public class Partner
            {
                public Partner partner { get; set; }
                public string PartnerNameArb                         { get; set; }
                public string PartnerNameEng                         { get; set; }
                public string PartnerSharePercentage                    { get; set; }
                public string EmiratesId                        { get; set; }
                public string Gender                            { get; set; }
                public string CompanyLicenseNo                                  { get; set; }
                public LicenseRelationship LicenseRelationship { get; set; }
                public PartnerNationality PartnerNationality { get; set; }
                public PartnerType PartnerType { get; set; }
            }
            public class Master
            {
                public Master master { get; set; }
                public string TradeLicenseNumber                { get; set; }
                public string BusinessNameArb               { get; set; }
                public string BusinessNameEng               { get; set; }
                public string BusinessLicenseBranchFlag                  { get; set; }
                public BranchType BranchType { get; set; }
                public string   MotherCompany                  { get; set; }
                public string   BusinesslicenseCityArb                    { get; set; }
                public string   BusinesslicenseCityEng                    { get; set; }
                public DateTime     EstablishmentDate                   { get; set; }
                public DateTime BusinessLicenseIssueDate                        { get; set; }
                public DateTime BusinessLicenseExpiryDate                   { get; set; }
                public string   CancelDate                                         { get; set; }
                public string   BusinessLicenseAddressArb                 { get; set; }
                public string   BusinessLicenseAddressEng                         { get; set; }
                public string    BuildingNumber                         { get; set; }
                public string   Building_Name                 { get; set; }
                public string CoordinatesX                  { get; set; }
                public string CoordinatesY                  { get; set; }
                public string ADCCIUnifiedID                            {           get; set; }
                public string LicenseRemarksArabic                      { get; set; }
                public LegalForm LegalForm { get; set; }
                public LicenseType LicenseType { get; set; }
                public EstablishmentCountry EstablishmentCountry { get; set; }
                public BusinessLicenseStatus BusinessLicenseStatus { get; set; }
                public Clasification Clasification { get; set; }
                public IssuePlace IssuePlace { get; set; }
                public string OfficialMobile                 { get; set; }
                public string OfficialEmail                             { get; set; }
                public string ProName                        { get; set; }
                public string PROMobileNumber                   { get; set; }
                public string ProEmail                  { get; set; }
                public string HasUpdatedRealBeneficiary                  { get; set; }
            }
            public class Master1
            {
                public List<Master1> master1 { get; set; }
                public string TradeLicenseNumber { get; set; }
                public string BusinessNameArb { get; set; }
                public string BusinessNameEng { get; set; }
                public string BusinessLicenseBranchFlag { get; set; }
                public BranchType BranchType { get; set; }
                public string MotherCompany { get; set; }
                public string BusinesslicenseCityArb { get; set; }
                public string BusinesslicenseCityEng { get; set; }
                public DateTime EstablishmentDate { get; set; }
                public DateTime BusinessLicenseIssueDate { get; set; }
                public DateTime BusinessLicenseExpiryDate { get; set; }
                public string CancelDate { get; set; }
                public string BusinessLicenseAddressArb { get; set; }
                public string BusinessLicenseAddressEng { get; set; }
                public string BuildingNumber { get; set; }
                public string Building_Name { get; set; }
                public string CoordinatesX { get; set; }
                public string CoordinatesY { get; set; }
                public string ADCCIUnifiedID { get; set; }
                public string LicenseRemarksArabic { get; set; }
                public LegalForm LegalForm { get; set; }
                public LicenseType LicenseType { get; set; }
                public EstablishmentCountry EstablishmentCountry { get; set; }
                public BusinessLicenseStatus BusinessLicenseStatus { get; set; }
                public Clasification Clasification { get; set; }
                public IssuePlace IssuePlace { get; set; }
                public string OfficialMobile { get; set; }
                public string OfficialEmail { get; set; }
                public string ProName { get; set; }
                public string PROMobileNumber { get; set; }
                public string ProEmail { get; set; }
                public string HasUpdatedRealBeneficiary { get; set; }
            }



            public class PartnerNationality
            {
                public string CountryCode                { get; set; }
                public string CountryNameArb            { get; set; }
                public string CountryNameEng                { get; set; }
            }

            public class PartnerType
            {
                public string PartnerTypeCode           { get; set; }
                public string PartnerTypeArb             { get; set; }
                public string PartnerTypeEng                { get; set; }
            }

            //public class Root
            //{
            //    [JsonProperty("?xml")]
            //    public Xml Xml { get; set; }

            //    [JsonProperty("env:Envelope")]
            //    public EnvEnvelope EnvEnvelope { get; set; }
            //}

            public class Xml
            {
                [JsonProperty("@version")]
                public string Version { get; set; }

                [JsonProperty("@encoding")]
                public string Encoding { get; set; }
            }
        }




        public class Partner1
        {
            public List<Partner1> partner1 { get; set; }
            public string PartnerNameArb                { get; set; }
            public string PartnerNameEng                { get; set; }
            public string PartnerSharePercentage                { get; set; }
            public string EmiratesId                { get; set; }
            public string Gender                    { get; set; }
            public string CompanyLicenseNo               { get; set; }
            public LicenseRelationship LicenseRelationship { get; set; }
            public PartnerNationality PartnerNationality { get; set; }
            public PartnerType PartnerType { get; set; }
        }
        public class PartnerDto
        {
            public string TradeLicenseNumber { get; set; }

            public string PartnerNameArb { get; set; }
            public string PartnerNameEng { get; set; }
            public string PartnerSharePercentage { get; set; }
            public string Gender { get; set; }
            public string CompanyLicenseNo { get; set; }
            public string LicenseRelationshipCode { get; set; }
            public string BusinessLicenseRelationshipArb { get; set; }
            public string BusinessLicenseRelationshipEng { get; set; }
            public string CountryCode { get; set; }
            public string CountryNameArb { get; set; }
            public string CountryNameEng { get; set; }
            public string PartnerTypeCode { get; set; }
            public string PartnerTypeArb { get; set; }
            public string PartnerTypeEng { get; set; }


        }
        public class MasterDto
        {
            public string BusinessNameArb { get; set; }
            public string BusinessNameEng { get; set; }
            public string BusinessLicenseBranchFlag { get; set; }
            public string BranchTypeCode { get; set; }
            public string BranchTypeArb { get; set; }
            public string BranchTypeEng { get; set; }
            public string MotherCompany { get; set; }
            public string BusinesslicenseCityArb { get; set; }
            public string BusinesslicenseCityEng { get; set; }
            public string EstablishmentDate { get; set; }
            public string BusinessLicenseIssueDate { get; set; }
            public string BusinessLicenseExpiryDate { get; set; }
            public string CancelDate { get; set; }
            public string BusinessLicenseAddressArb { get; set; }
            public string BusinessLicenseAddressEng { get; set; }
            public string BuildingNumber { get; set; }
            public string Building_Name { get; set; }
            public string CoordinatesX { get; set; }
            public string CoordinatesY { get; set; }
            public string ADCCIUnifiedID { get; set; }
            public string LicenseRemarksArabic { get; set; }
            public string LegalFormCode { get; set; }
            public string BusinessLegalFormArb { get; set; }
            public string BusinessLegalFormEng { get; set; }
            public string LicenseTypeCode { get; set; }
            public string BusinessLicenseTypeArb { get; set; }
            public string BusinessLicenseTypeEng { get; set; }
            public string CountryCode { get; set; }
            public string CountryNameArb { get; set; }
            public string CountryNameEng { get; set; }
            public string BusinessLicenseStatusCode { get; set; }
            public string BusinessLicenseStatusArb { get; set; }
            public string BusinessLicenseStatusEng { get; set; }
            public string ClasificationCode { get; set; }
            public string ClasificationArb { get; set; }
            public string ClasificationEng { get; set; }
            public string IssuePlaceCode { get; set; }
            public string IssuePlaceArb { get; set; }
            public string IssuePlaceEng { get; set; }
            public string OfficialMobile { get; set; }
            public string OfficialEmail { get; set; }
            public string ProName { get; set; }
            public string PROMobileNumber { get; set; }
            public string ProEmail { get; set; }
            public string HasUpdatedRealBeneficiary { get; set; }
        }
        public class ADDEDDetailsRequestParams
        {
            public string EmiratesId { get; set; }
        }

        public class ADDEDDetailsResponseParams
        {
            public string TradeLicenseNumber { get; set; }
            public string BusinessNameArb { get; set; }
            public string BusinessLicenseIssueDate { get; set; }
            public string BusinessLicenseExpiryDate { get; set; }
            public string EstablishmentDate { get; set; }
            public string BusinessLicenseTypeArb { get; set; }
            public List<Partners> Partner { get; set; }
            public List<Activities> Activity { get; set; }
        }

        public class Partners
        {
            public string PartnerNameArb { get; set; }
            public string LicenseRelationshipCode { get; set; }
            public string BusinessLicenseRelationshipArb { get; set; }
            public string PartnerSharePercentage { get; set; }
        }

        public class Activities
        {
            public string ActivityNameArb { get; set; }
            public string ActivityNameEng { get; set; }

        }

        public class LicenseDetailsResponse
        {
            public List<MasterDto> MasterDto { get; set; }
            public List<PartnerDto> PartnerDto { get; set; }
            public List<Activity> Activity { get; set; }
        }

    }
}