using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace MOCDIntegrations.Models
{
    public class ADJDAttestation
    {
        public class ADJDContent
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            //public class AttestationListResponse
            //{
            //    [JsonProperty("@xmlns")]
            //    public string xmlns { get; set; }

            //    [JsonProperty("@xmlns:ns0")]
            //    public string xmlnsns0 { get; set; }
            //    public List<Content> Contents { get; set; }
            //}

            //public class Content
            //{
            //    [JsonProperty("ns0:DocumentNumber")]
            //    public string ns0DocumentNumber { get; set; }

            //    [JsonProperty("ns0:DocumentType")]
            //    public Ns0DocumentType ns0DocumentType { get; set; }

            //    [JsonProperty("ns0:PartyRole")]
            //    public Ns0PartyRole ns0PartyRole { get; set; }

            //    [JsonProperty("ns0:DocumentSubType")]
            //    public Ns0DocumentSubType ns0DocumentSubType { get; set; }

            //    [JsonProperty("ns0:DocumentReferenceNumber")]
            //    public string ns0DocumentReferenceNumber { get; set; }

            //    [JsonProperty("ns0:WithAttestation")]
            //    public string ns0WithAttestation { get; set; }

            //    [JsonProperty("ns0:WithExecutiveFormula")]
            //    public string ns0WithExecutiveFormula { get; set; }
            //}

            //public class Ns0DocumentSubType
            //{
            //    [JsonProperty("ns0:Code")]
            //    public string ns0Code { get; set; }

            //    [JsonProperty("ns0:DescriptionEnglish")]
            //    public string ns0DescriptionEnglish { get; set; }

            //    [JsonProperty("ns0:DescriptionArabic")]
            //    public string ns0DescriptionArabic { get; set; }
            //}

            //public class Ns0DocumentType
            //{
            //    [JsonProperty("ns0:Code")]
            //    public string ns0Code { get; set; }

            //    [JsonProperty("ns0:DescriptionEnglish")]
            //    public string ns0DescriptionEnglish { get; set; }

            //    [JsonProperty("ns0:DescriptionArabic")]
            //    public string ns0DescriptionArabic { get; set; }
            //}

            //public class Ns0PartyRole
            //{
            //    [JsonProperty("ns0:Code")]
            //    public string ns0Code { get; set; }

            //    [JsonProperty("ns0:DescriptionEnglish")]
            //    public string ns0DescriptionEnglish { get; set; }

            //    [JsonProperty("ns0:DescriptionArabic")]
            //    public string ns0DescriptionArabic { get; set; }
            //}

            //public class Root
            //{
            //    [JsonProperty("?xml")]
            //    public Xml xml { get; set; }

            //    [JsonProperty("soapenv:Envelope")]
            //    public SoapenvEnvelope soapenvEnvelope { get; set; }
            //}

            //public class SBody
            //{
            //    [JsonProperty("@xmlns:env")]
            //    public string xmlnsenv { get; set; }

            //    [JsonProperty("@xmlns:S")]
            //    public string xmlnsS { get; set; }
            //    public AttestationListResponse AttestationListResponse { get; set; }
            //}

            //public class SHeader
            //{
            //    [JsonProperty("@xmlns:env")]
            //    public string xmlnsenv { get; set; }

            //    [JsonProperty("@xmlns:S")]
            //    public string xmlnsS { get; set; }

            //    [JsonProperty("urn:ResponseTrnHeader")]
            //    public UrnResponseTrnHeader urnResponseTrnHeader { get; set; }
            //}

            //public class SoapenvEnvelope
            //{
            //    [JsonProperty("@xmlns:soapenv")]
            //    public string xmlnssoapenv { get; set; }

            //    [JsonProperty("S:Header")]
            //    public SHeader SHeader { get; set; }

            //    [JsonProperty("S:Body")]
            //    public SBody SBody { get; set; }
            //}

            //public class UrnResponseTrnHeader
            //{
            //    [JsonProperty("@xmlns:urn")]
            //    public string xmlnsurn { get; set; }

            //    [JsonProperty("@xmlns:urn1")]
            //    public string xmlnsurn1 { get; set; }

            //    [JsonProperty("urn1:EntityCode")]
            //    public string urn1EntityCode { get; set; }

            //    [JsonProperty("urn1:TransactionId")]
            //    public string urn1TransactionId { get; set; }
            //}

            //public class Xml
            //{
            //    [JsonProperty("@version")]
            //    public string version { get; set; }

            //    [JsonProperty("@encoding")]
            //    public string encoding { get; set; }
            //}
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            public class ATTDocumentNumber
            {
                [JsonProperty("@xmlns:ATT")]
                public string xmlnsATT { get; set; }

                [JsonProperty("#text")]
                public string text { get; set; }
            }

            public class ATTDocumentReferenceNumber
            {
                [JsonProperty("@xmlns:ATT")]
                public string xmlnsATT { get; set; }

                [JsonProperty("#text")]
                public string text { get; set; }
            }

            public class ATTDocumentSubType
            {
                [JsonProperty("@xmlns:ATT")]
                public string xmlnsATT { get; set; }

                [JsonProperty("ATT:Code")]
                public string ATTCode { get; set; }

                [JsonProperty("ATT:DescriptionEnglish")]
                public string ATTDescriptionEnglish { get; set; }

                [JsonProperty("ATT:DescriptionArabic")]
                public string ATTDescriptionArabic { get; set; }
            }

            public class ATTDocumentType
            {
                [JsonProperty("@xmlns:ATT")]
                public string xmlnsATT { get; set; }

                [JsonProperty("ATT:Code")]
                public string ATTCode { get; set; }

                [JsonProperty("ATT:DescriptionEnglish")]
                public string ATTDescriptionEnglish { get; set; }

                [JsonProperty("ATT:DescriptionArabic")]
                public string ATTDescriptionArabic { get; set; }
            }

            public class ATTPartyRole
            {
                [JsonProperty("@xmlns:ATT")]
                public string xmlnsATT { get; set; }

                [JsonProperty("ATT:Code")]
                public string ATTCode { get; set; }

                [JsonProperty("ATT:DescriptionEnglish")]
                public string ATTDescriptionEnglish { get; set; }

                [JsonProperty("ATT:DescriptionArabic")]
                public string ATTDescriptionArabic { get; set; }
            }

            public class ATTWithAttestation
            {
                [JsonProperty("@xmlns:ATT")]
                public string xmlnsATT { get; set; }

                [JsonProperty("#text")]
                public string text { get; set; }
            }

            public class ATTWithExecutiveFormula
            {
                [JsonProperty("@xmlns:ATT")]
                public string xmlnsATT { get; set; }

                [JsonProperty("#text")]
                public string text { get; set; }
            }

            public class Ns2AttestationListResponse
            {
                [JsonProperty("@xmlns:ns2")]
                public string xmlnsns2 { get; set; }

                [JsonProperty("ns2:Contents")]
                public List<Ns2Content> ns2Contents { get; set; }
            }

            public class Ns2Content
            {
                [JsonProperty("ATT:DocumentNumber")]
                public ATTDocumentNumber ATTDocumentNumber { get; set; }

                [JsonProperty("ATT:DocumentType")]
                public ATTDocumentType ATTDocumentType { get; set; }

                [JsonProperty("ATT:PartyRole")]
                public ATTPartyRole ATTPartyRole { get; set; }

                [JsonProperty("ATT:DocumentSubType")]
                public ATTDocumentSubType ATTDocumentSubType { get; set; }

                [JsonProperty("ATT:DocumentReferenceNumber")]
                public ATTDocumentReferenceNumber ATTDocumentReferenceNumber { get; set; }

                [JsonProperty("ATT:WithAttestation")]
                public ATTWithAttestation ATTWithAttestation { get; set; }

                [JsonProperty("ATT:WithExecutiveFormula")]
                public ATTWithExecutiveFormula ATTWithExecutiveFormula { get; set; }
            }

            public class Root
            {
                [JsonProperty("?xml")]
                public Xml xml { get; set; }

                [JsonProperty("soapenv:Envelope")]
                public SoapenvEnvelope soapenvEnvelope { get; set; }
            }

            public class SoapenvBody
            {
                [JsonProperty("ns2:AttestationListResponse")]
                public Ns2AttestationListResponse ns2AttestationListResponse { get; set; }
            }

            public class SoapenvEnvelope
            {
                [JsonProperty("@xmlns:soapenv")]
                public string xmlnssoapenv { get; set; }

                [JsonProperty("soapenv:Header")]
                public SoapenvHeader soapenvHeader { get; set; }

                [JsonProperty("soapenv:Body")]
                public SoapenvBody soapenvBody { get; set; }
            }

            public class SoapenvHeader
            {
                [JsonProperty("urn:ResponseTrnHeader")]
                public UrnResponseTrnHeader urnResponseTrnHeader { get; set; }
            }

            public class UrnResponseTrnHeader
            {
                [JsonProperty("@xmlns:urn")]
                public string xmlnsurn { get; set; }

                [JsonProperty("@xmlns:urn1")]
                public string xmlnsurn1 { get; set; }

                [JsonProperty("urn1:EntityCode")]
                public string urn1EntityCode { get; set; }

                [JsonProperty("urn1:TransactionId")]
                public string urn1TransactionId { get; set; }
            }

            public class Xml
            {
                [JsonProperty("@version")]
                public string version { get; set; }

                [JsonProperty("@encoding")]
                public string encoding { get; set; }
            }




        }
        public class ADJDDocumentDetails
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            public class AttestationDetailsResponse
            {
                [JsonProperty("@xmlns")]
                public string xmlns { get; set; }

                [JsonProperty("@xmlns:ns0")]
                public string xmlnsns0 { get; set; }
                public Contents Contents { get; set; }
            }

            public class Contents
            {
                [JsonProperty("ns0:DocumentInfo")]
                public Ns0DocumentInfo ns0DocumentInfo { get; set; }

                [JsonProperty("ns0:DocumentDetails")]
                public Ns0DocumentDetails ns0DocumentDetails { get; set; }

                [JsonProperty("ns0:PartyDetails")]
                public List<Ns0PartyDetail> ns0PartyDetails { get; set; }
            }

            public class Ns0DocumentDetails
            {
                [JsonProperty("ns0:ConcernedSite1")]
                public string ns0ConcernedSite1 { get; set; }

                [JsonProperty("ns0:Judge")]
                public string ns0Judge { get; set; }

                [JsonProperty("ns0:ClientText")]
                public string ns0ClientText { get; set; }

                [JsonProperty("ns0:DocumentText")]
                public string ns0DocumentText { get; set; }

                [JsonProperty("ns0:FreeofCharge")]
                public string ns0FreeofCharge { get; set; }

                [JsonProperty("ns0:FolderNumber")]
                public string ns0FolderNumber { get; set; }

                [JsonProperty("ns0:HijriDate")]
                public string ns0HijriDate { get; set; }

                [JsonProperty("ns0:Branch")]
                public string ns0Branch { get; set; }

                [JsonProperty("ns0:BranchCode")]
                public string ns0BranchCode { get; set; }

                [JsonProperty("ns0:Owner")]
                public string ns0Owner { get; set; }

                [JsonProperty("ns0:WithAttestation")]
                public string ns0WithAttestation { get; set; }

                [JsonProperty("ns0:WithExecutiveFormula")]
                public string ns0WithExecutiveFormula { get; set; }
            }

            public class Ns0DocumentInfo
            {
                [JsonProperty("ns0:DocumentType")]
                public Ns0DocumentType ns0DocumentType { get; set; }

                [JsonProperty("ns0:DocumentSubType")]
                public Ns0DocumentSubType ns0DocumentSubType { get; set; }

                [JsonProperty("ns0:DocumentDate")]
                public string ns0DocumentDate { get; set; }

                [JsonProperty("ns0:DocumentNumber")]
                public string ns0DocumentNumber { get; set; }

                [JsonProperty("ns0:DocumentReferenceNumber")]
                public string ns0DocumentReferenceNumber { get; set; }
            }

            public class Ns0DocumentSubType
            {
                [JsonProperty("ns0:Code")]
                public string ns0Code { get; set; }

                [JsonProperty("ns0:DescriptionEnglish")]
                public string ns0DescriptionEnglish { get; set; }

                [JsonProperty("ns0:DescriptionArabic")]
                public string ns0DescriptionArabic { get; set; }
            }

            public class Ns0DocumentType
            {
                [JsonProperty("ns0:Code")]
                public string ns0Code { get; set; }

                [JsonProperty("ns0:DescriptionEnglish")]
                public string ns0DescriptionEnglish { get; set; }

                [JsonProperty("ns0:DescriptionArabic")]
                public string ns0DescriptionArabic { get; set; }
            }

            public class Ns0Gender
            {
                [JsonProperty("ns0:Code")]
                public string ns0Code { get; set; }

                [JsonProperty("ns0:DescriptionEnglish")]
                public string ns0DescriptionEnglish { get; set; }

                [JsonProperty("ns0:DescriptionArabic")]
                public string ns0DescriptionArabic { get; set; }
            }

            public class Ns0HeirDetails
            {
                [JsonProperty("ns0:HeirRole")]
                public object ns0HeirRole { get; set; }
            }

            public class Ns0IDType
            {
                [JsonProperty("ns0:Code")]
                public string ns0Code { get; set; }

                [JsonProperty("ns0:DescriptionEnglish")]
                public string ns0DescriptionEnglish { get; set; }

                [JsonProperty("ns0:DescriptionArabic")]
                public string ns0DescriptionArabic { get; set; }
            }

            public class Ns0Nationality
            {
                [JsonProperty("ns0:Code")]
                public string ns0Code { get; set; }

                [JsonProperty("ns0:DescriptionEnglish")]
                public string ns0DescriptionEnglish { get; set; }

                [JsonProperty("ns0:DescriptionArabic")]
                public string ns0DescriptionArabic { get; set; }
            }

            public class Ns0PartyDetail
            {
                [JsonProperty("ns0:PartyRole")]
                public Ns0PartyRole ns0PartyRole { get; set; }

                [JsonProperty("ns0:FirstName")]
                public string ns0FirstName { get; set; }

                [JsonProperty("ns0:SecondName")]
                public string ns0SecondName { get; set; }

                [JsonProperty("ns0:ThirdName")]
                public string ns0ThirdName { get; set; }

                [JsonProperty("ns0:FamilyName")]
                public string ns0FamilyName { get; set; }

                [JsonProperty("ns0:DateofBirth")]
                public string ns0DateofBirth { get; set; }

                [JsonProperty("ns0:Gender")]
                public Ns0Gender ns0Gender { get; set; }

                [JsonProperty("ns0:Nationality")]
                public Ns0Nationality ns0Nationality { get; set; }

                [JsonProperty("ns0:Religion")]
                public Ns0Religion ns0Religion { get; set; }

                [JsonProperty("ns0:Telephone")]
                public string ns0Telephone { get; set; }

                [JsonProperty("ns0:IDType")]
                public Ns0IDType ns0IDType { get; set; }

                [JsonProperty("ns0:IDNumber")]
                public string ns0IDNumber { get; set; }

                [JsonProperty("ns0:PlaceofIssue")]
                public string ns0PlaceofIssue { get; set; }

                [JsonProperty("ns0:HeirDetails")]
                public Ns0HeirDetails ns0HeirDetails { get; set; }

                [JsonProperty("ns0:FourthName")]
                public string ns0FourthName { get; set; }
            }

            public class Ns0PartyRole
            {
                [JsonProperty("ns0:Code")]
                public string ns0Code { get; set; }

                [JsonProperty("ns0:DescriptionEnglish")]
                public string ns0DescriptionEnglish { get; set; }

                [JsonProperty("ns0:DescriptionArabic")]
                public string ns0DescriptionArabic { get; set; }
            }

            public class Ns0Religion
            {
                [JsonProperty("ns0:Code")]
                public string ns0Code { get; set; }

                [JsonProperty("ns0:DescriptionEnglish")]
                public string ns0DescriptionEnglish { get; set; }

                [JsonProperty("ns0:DescriptionArabic")]
                public string ns0DescriptionArabic { get; set; }
            }

            public class Root
            {
                [JsonProperty("?xml")]
                public Xml xml { get; set; }

                [JsonProperty("soapenv:Envelope")]
                public SoapenvEnvelope soapenvEnvelope { get; set; }
            }

            public class SBody
            {
                [JsonProperty("@xmlns:env")]
                public string xmlnsenv { get; set; }

                [JsonProperty("@xmlns:S")]
                public string xmlnsS { get; set; }
                public AttestationDetailsResponse AttestationDetailsResponse { get; set; }
            }

            public class SHeader
            {
                [JsonProperty("@xmlns:env")]
                public string xmlnsenv { get; set; }

                [JsonProperty("@xmlns:S")]
                public string xmlnsS { get; set; }

                [JsonProperty("urn:ResponseTrnHeader")]
                public UrnResponseTrnHeader urnResponseTrnHeader { get; set; }
            }

            public class SoapenvEnvelope
            {
                [JsonProperty("@xmlns:soapenv")]
                public string xmlnssoapenv { get; set; }

                [JsonProperty("S:Header")]
                public SHeader SHeader { get; set; }

                [JsonProperty("S:Body")]
                public SBody SBody { get; set; }
            }

            public class UrnResponseTrnHeader
            {
                [JsonProperty("@xmlns:urn")]
                public string xmlnsurn { get; set; }

                [JsonProperty("@xmlns:urn1")]
                public string xmlnsurn1 { get; set; }

                [JsonProperty("urn1:EntityCode")]
                public string urn1EntityCode { get; set; }

                [JsonProperty("urn1:TransactionId")]
                public string urn1TransactionId { get; set; }
            }

            public class Xml
            {
                [JsonProperty("@version")]
                public string version { get; set; }

                [JsonProperty("@encoding")]
                public string encoding { get; set; }
            }




        }
    }

public class ADJDCompleteDetails
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class ATTDocumentDetails
        {
            [JsonProperty("@xmlns:ATT")]
            public string xmlnsATT { get; set; }

            [JsonProperty("ATT:ConcernedSite1")]
            public string ATTConcernedSite1 { get; set; }

            [JsonProperty("ATT:Judge")]
            public string ATTJudge { get; set; }

            [JsonProperty("ATT:ClientText")]
            public string ATTClientText { get; set; }

            [JsonProperty("ATT:DocumentText")]
            public string ATTDocumentText { get; set; }

            [JsonProperty("ATT:FreeofCharge")]
            public string ATTFreeofCharge { get; set; }

            [JsonProperty("ATT:Witnessisknown")]
            public string ATTWitnessisknown { get; set; }

            [JsonProperty("ATT:FolderNumber")]
            public string ATTFolderNumber { get; set; }

            [JsonProperty("ATT:HijriDate")]
            public string ATTHijriDate { get; set; }

            [JsonProperty("ATT:Branch")]
            public string ATTBranch { get; set; }

            [JsonProperty("ATT:BranchCode")]
            public string ATTBranchCode { get; set; }

            [JsonProperty("ATT:Owner")]
            public string ATTOwner { get; set; }

            [JsonProperty("ATT:WithAttestation")]
            public string ATTWithAttestation { get; set; }

            [JsonProperty("ATT:WithExecutiveFormula")]
            public string ATTWithExecutiveFormula { get; set; }
        }

        public class ATTDocumentInfo
        {
            [JsonProperty("@xmlns:ATT")]
            public string xmlnsATT { get; set; }

            [JsonProperty("ATT:DocumentType")]
            public ATTDocumentType ATTDocumentType { get; set; }

            [JsonProperty("ATT:DocumentSubType")]
            public ATTDocumentSubType ATTDocumentSubType { get; set; }

            [JsonProperty("ATT:DocumentDate")]
            public string ATTDocumentDate { get; set; }

            [JsonProperty("ATT:DocumentNumber")]
            public string ATTDocumentNumber { get; set; }

            [JsonProperty("ATT:DocumentReferenceNumber")]
            public string ATTDocumentReferenceNumber { get; set; }
        }

        public class ATTDocumentSubType
        {
            [JsonProperty("ATT:Code")]
            public string ATTCode { get; set; }

            [JsonProperty("ATT:DescriptionEnglish")]
            public string ATTDescriptionEnglish { get; set; }

            [JsonProperty("ATT:DescriptionArabic")]
            public string ATTDescriptionArabic { get; set; }
        }

        public class ATTDocumentType
        {
            [JsonProperty("ATT:Code")]
            public string ATTCode { get; set; }

            [JsonProperty("ATT:DescriptionEnglish")]
            public string ATTDescriptionEnglish { get; set; }

            [JsonProperty("ATT:DescriptionArabic")]
            public string ATTDescriptionArabic { get; set; }
        }

        public class ATTGender
        {
            [JsonProperty("ATT:Code")]
            public string ATTCode { get; set; }

            [JsonProperty("ATT:DescriptionEnglish")]
            public string ATTDescriptionEnglish { get; set; }

            [JsonProperty("ATT:DescriptionArabic")]
            public string ATTDescriptionArabic { get; set; }
        }

        public class ATTHeirDetails
        {
            [JsonProperty("ATT:HeirRole")]
            public object ATTHeirRole { get; set; }
        }

        public class ATTIDType
        {
            [JsonProperty("ATT:Code")]
            public string ATTCode { get; set; }

            [JsonProperty("ATT:DescriptionEnglish")]
            public string ATTDescriptionEnglish { get; set; }

            [JsonProperty("ATT:DescriptionArabic")]
            public string ATTDescriptionArabic { get; set; }
        }

        public class ATTNationality
        {
            [JsonProperty("ATT:Code")]
            public string ATTCode { get; set; }

            [JsonProperty("ATT:DescriptionEnglish")]
            public string ATTDescriptionEnglish { get; set; }

            [JsonProperty("ATT:DescriptionArabic")]
            public string ATTDescriptionArabic { get; set; }
        }

        public class ATTPartyRole
        {
            [JsonProperty("ATT:Code")]
            public string ATTCode { get; set; }

            [JsonProperty("ATT:DescriptionEnglish")]
            public string ATTDescriptionEnglish { get; set; }

            [JsonProperty("ATT:DescriptionArabic")]
            public string ATTDescriptionArabic { get; set; }
        }

        public class ATTReligion
        {
            [JsonProperty("ATT:Code")]
            public string ATTCode { get; set; }

            [JsonProperty("ATT:DescriptionEnglish")]
            public string ATTDescriptionEnglish { get; set; }

            [JsonProperty("ATT:DescriptionArabic")]
            public string ATTDescriptionArabic { get; set; }
        }

        public class Ns2AttestationDetailsResponse
        {
            [JsonProperty("@xmlns:ns2")]
            public string xmlnsns2 { get; set; }

            [JsonProperty("ns2:Contents")]
            public Ns2Contents ns2Contents { get; set; }
        }

        public class Ns2Contents
        {
            [JsonProperty("ATT:DocumentInfo")]
            public ATTDocumentInfo ATTDocumentInfo { get; set; }

            [JsonProperty("ATT:DocumentDetails")]
            public ATTDocumentDetails ATTDocumentDetails { get; set; }
            public List<PartyDetail> PartyDetails { get; set; }
        }

        public class PartyDetail
        {
            [JsonProperty("@xmlns:ATT")]
            public string xmlnsATT { get; set; }

            [JsonProperty("ATT:PartyRole")]
            public ATTPartyRole ATTPartyRole { get; set; }

            [JsonProperty("ATT:FirstName")]
            public string ATTFirstName { get; set; }

            [JsonProperty("ATT:SecondName")]
            public string ATTSecondName { get; set; }

            [JsonProperty("ATT:ThirdName")]
            public string ATTThirdName { get; set; }

            [JsonProperty("ATT:FourthName")]
            public string ATTFourthName { get; set; }

            [JsonProperty("ATT:FamilyName")]
            public string ATTFamilyName { get; set; }

            [JsonProperty("ATT:DateofBirth")]
            public string ATTDateofBirth { get; set; }

            [JsonProperty("ATT:Gender")]
            public ATTGender ATTGender { get; set; }

            [JsonProperty("ATT:Nationality")]
            public ATTNationality ATTNationality { get; set; }

            [JsonProperty("ATT:Religion")]
            public ATTReligion ATTReligion { get; set; }

            [JsonProperty("ATT:Telephone")]
            public string ATTTelephone { get; set; }

            [JsonProperty("ATT:IDType")]
            public ATTIDType ATTIDType { get; set; }

            [JsonProperty("ATT:IDNumber")]
            public string ATTIDNumber { get; set; }

            [JsonProperty("ATT:IDIssueDate")]
            public string ATTIDIssueDate { get; set; }

            [JsonProperty("ATT:PlaceofIssue")]
            public string ATTPlaceofIssue { get; set; }

            [JsonProperty("ATT:HeirDetails")]
            public ATTHeirDetails ATTHeirDetails { get; set; }
        }

        public class Root
        {
            [JsonProperty("?xml")]
            public Xml xml { get; set; }

            [JsonProperty("soapenv:Envelope")]
            public SoapenvEnvelope soapenvEnvelope { get; set; }
        }

        public class SoapenvBody
        {
            [JsonProperty("ns2:AttestationDetailsResponse")]
            public Ns2AttestationDetailsResponse ns2AttestationDetailsResponse { get; set; }
        }

        public class SoapenvEnvelope
        {
            [JsonProperty("@xmlns:soapenv")]
            public string xmlnssoapenv { get; set; }

            [JsonProperty("soapenv:Header")]
            public SoapenvHeader soapenvHeader { get; set; }

            [JsonProperty("soapenv:Body")]
            public SoapenvBody soapenvBody { get; set; }
        }

        public class SoapenvHeader
        {
            [JsonProperty("urn:ResponseTrnHeader")]
            public UrnResponseTrnHeader urnResponseTrnHeader { get; set; }
        }

        public class UrnResponseTrnHeader
        {
            [JsonProperty("@xmlns:urn")]
            public string xmlnsurn { get; set; }

            [JsonProperty("@xmlns:urn1")]
            public string xmlnsurn1 { get; set; }

            [JsonProperty("urn1:EntityCode")]
            public string urn1EntityCode { get; set; }

            [JsonProperty("urn1:TransactionId")]
            public string urn1TransactionId { get; set; }
        }

        public class Xml
        {
            [JsonProperty("@version")]
            public string version { get; set; }

            [JsonProperty("@encoding")]
            public string encoding { get; set; }
        }




    }
}