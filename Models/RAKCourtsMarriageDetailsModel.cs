using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace MOCDIntegrations.Models
{
    public class RAKCourtsMarriageDetailsModel
    {
        public class DocumentEnvelope
        {

            public class Attachment
            {
                public Item item { get; set; }
            }

            public class Item
            {
                public string ZDocTypeCode { get; set; }
                public string ZDocArabicDesc { get; set; }
                public string ZDocEnglishDesc { get; set; }
                public string ZDocName { get; set; }
                public string ZDocCreationAt { get; set; }
                public string ZDocument { get; set; }
                public string ZTransactionId { get; set; }
            }

            public class Ns1MTContractDocumentsRes
            {
                [JsonProperty("@xmlns:ns1")]
                public string xmlnsns1 { get; set; }
                public Attachment Attachment { get; set; }
                public ResponseCode Response_Code { get; set; }
            }

            public class ResponseCode
            {
                public string Response_Code { get; set; }
                public string Response_Description_Ar { get; set; }
                public string Response_Description_En { get; set; }
            }

            public class Root
            {
                [JsonProperty("?xml")]
                public Xml xml { get; set; }

                [JsonProperty("SOAP:Envelope")]
                public SOAPEnvelope SOAPEnvelope { get; set; }
            }

            public class SOAPBody
            {
                [JsonProperty("ns1:MT_ContractDocuments_Res")]
                public Ns1MTContractDocumentsRes ns1MT_ContractDocuments_Res { get; set; }
            }

            public class SOAPEnvelope
            {
                [JsonProperty("@xmlns:SOAP")]
                public string xmlnsSOAP { get; set; }

                [JsonProperty("SOAP:Header")]
                public object SOAPHeader { get; set; }

                [JsonProperty("SOAP:Body")]
                public SOAPBody SOAPBody { get; set; }
            }

            public class Xml
            {
                [JsonProperty("@version")]
                public string version { get; set; }

                [JsonProperty("@encoding")]
                public string encoding { get; set; }
            }


        }

        public class DocucmentDetailsReponse
        {
            public string MarriageDetailsID { get; set; }
            public string ZTransactionId { get; set; }

            public string ZDocTypeCode { get; set; }
            public string ZDocArabicDesc { get; set; }
            public string ZDocEnglishDesc { get; set; }
            public string ZDocName { get; set; }
            public string ZDocCreationAt { get; set; }
            public string ZDocument { get; set; }
            public string Response_Code { get; set; }
            public string Response_Description_Ar { get; set; }
            public string Response_Description_En { get; set; }

        }
    }
    public class RAKEnvelope
    {

        // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
        public partial class Envelope
        {

            private object headerField;

            private EnvelopeBody bodyField;

            /// <remarks/>
            public object Header
            {
                get
                {
                    return this.headerField;
                }
                set
                {
                    this.headerField = value;
                }
            }

            /// <remarks/>
            public EnvelopeBody Body
            {
                get
                {
                    return this.bodyField;
                }
                set
                {
                    this.bodyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public partial class EnvelopeBody
        {

            private MT_MarriageDetails_Res mT_MarriageDetails_ResField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://ega.rak.ae/xi/MarriageandDivorceInquiry")]
            public MT_MarriageDetails_Res MT_MarriageDetails_Res
            {
                get
                {
                    return this.mT_MarriageDetails_ResField;
                }
                set
                {
                    this.mT_MarriageDetails_ResField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ega.rak.ae/xi/MarriageandDivorceInquiry")]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ega.rak.ae/xi/MarriageandDivorceInquiry", IsNullable = false)]
        public partial class MT_MarriageDetails_Res
        {

            private MarriageItem[] marriageField;

            private Response_Code response_CodeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayAttribute(Namespace = "")]
            [System.Xml.Serialization.XmlArrayItemAttribute("item", IsNullable = false)]
            public MarriageItem[] Marriage
            {
                get
                {
                    return this.marriageField;
                }
                set
                {
                    this.marriageField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
            public Response_Code Response_Code
            {
                get
                {
                    return this.response_CodeField;
                }
                set
                {
                    this.response_CodeField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class MarriageItem
        {

            private string zTransactionIdField;
            private string zMarriageContractNumField;
            private string zMarriageContractYearField;
            private string zContractDateField;
            private string zContractDateHijriField;
            private string zActualMarriageDateField;
            private string zActualMarriageDateHijriField;
            private string zHusbandNameArField;
            private string zHusbandEidField;
            private string zHusbandPassportNumField;
            private string zHusbandPassportCouField;
            private string zHusbandKhulasitQaidField;
            private string zHusbandDOBField;
            private string zHusbandNatField;
            private string zHusbandRelField;
            private string zWifeNameArField;
            private string zWifeEidField;
            private string zWifePassportNumField;
            private string zWifePassportCouField;
            private object zWifeKhulasitQaidField;
            private string zWifeDOBField;
            private string zWifeNatField;
            private string zWifeRelField;

            /// <remarks/>
            public string ZTransactionId
            {
                get
                {
                    return this.zTransactionIdField;
                }
                set
                {
                    this.zTransactionIdField = value;
                }
            }

            /// <remarks/>
            public string ZMarriageContractNum
            {
                get
                {
                    return this.zMarriageContractNumField;
                }
                set
                {
                    this.zMarriageContractNumField = value;
                }
            }

            /// <remarks/>
            public string ZMarriageContractYear
            {
                get
                {
                    return this.zMarriageContractYearField;
                }
                set
                {
                    this.zMarriageContractYearField = value;
                }
            }

            /// <remarks/>
            public string ZContractDate
            {
                get
                {
                    return this.zContractDateField;
                }
                set
                {
                    this.zContractDateField = value;
                }
            }

            /// <remarks/>
            public string ZContractDateHijri
            {
                get
                {
                    return this.zContractDateHijriField;
                }
                set
                {
                    this.zContractDateHijriField = value;
                }
            }

            /// <remarks/>
            public string ZActualMarriageDate
            {
                get
                {
                    return this.zActualMarriageDateField;
                }
                set
                {
                    this.zActualMarriageDateField = value;
                }
            }

            /// <remarks/>
            public string ZActualMarriageDateHijri
            {
                get
                {
                    return this.zActualMarriageDateHijriField;
                }
                set
                {
                    this.zActualMarriageDateHijriField = value;
                }
            }

            /// <remarks/>
            public string ZHusbandNameAr
            {
                get
                {
                    return this.zHusbandNameArField;
                }
                set
                {
                    this.zHusbandNameArField = value;
                }
            }

            /// <remarks/>
            public string ZHusbandEid
            {
                get
                {
                    return this.zHusbandEidField;
                }
                set
                {
                    this.zHusbandEidField = value;
                }
            }

            /// <remarks/>
            public string ZHusbandPassportNum
            {
                get
                {
                    return this.zHusbandPassportNumField;
                }
                set
                {
                    this.zHusbandPassportNumField = value;
                }
            }

            /// <remarks/>
            public string ZHusbandPassportCou
            {
                get
                {
                    return this.zHusbandPassportCouField;
                }
                set
                {
                    this.zHusbandPassportCouField = value;
                }
            }

            /// <remarks/>
            public string ZHusbandKhulasitQaid
            {
                get
                {
                    return this.zHusbandKhulasitQaidField;
                }
                set
                {
                    this.zHusbandKhulasitQaidField = value;
                }
            }

            /// <remarks/>
            public string ZHusbandDOB
            {
                get
                {
                    return this.zHusbandDOBField;
                }
                set
                {
                    this.zHusbandDOBField = value;
                }
            }

            /// <remarks/>
            public string ZHusbandNat
            {
                get
                {
                    return this.zHusbandNatField;
                }
                set
                {
                    this.zHusbandNatField = value;
                }
            }

            /// <remarks/>
            public string ZHusbandRel
            {
                get
                {
                    return this.zHusbandRelField;
                }
                set
                {
                    this.zHusbandRelField = value;
                }
            }

            /// <remarks/>
            public string ZWifeNameAr
            {
                get
                {
                    return this.zWifeNameArField;
                }
                set
                {
                    this.zWifeNameArField = value;
                }
            }

            /// <remarks/>
            public string ZWifeEid
            {
                get
                {
                    return this.zWifeEidField;
                }
                set
                {
                    this.zWifeEidField = value;
                }
            }

            /// <remarks/>
            public string ZWifePassportNum
            {
                get
                {
                    return this.zWifePassportNumField;
                }
                set
                {
                    this.zWifePassportNumField = value;
                }
            }

            /// <remarks/>
            public string ZWifePassportCou
            {
                get
                {
                    return this.zWifePassportCouField;
                }
                set
                {
                    this.zWifePassportCouField = value;
                }
            }

            /// <remarks/>
            public object ZWifeKhulasitQaid
            {
                get
                {
                    return this.zWifeKhulasitQaidField;
                }
                set
                {
                    this.zWifeKhulasitQaidField = value;
                }
            }

            /// <remarks/>
            public string ZWifeDOB
            {
                get
                {
                    return this.zWifeDOBField;
                }
                set
                {
                    this.zWifeDOBField = value;
                }
            }

            /// <remarks/>
            public string ZWifeNat
            {
                get
                {
                    return this.zWifeNatField;
                }
                set
                {
                    this.zWifeNatField = value;
                }
            }

            /// <remarks/>
            public string ZWifeRel
            {
                get
                {
                    return this.zWifeRelField;
                }
                set
                {
                    this.zWifeRelField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Response_Code
        {

            private byte response_Code1Field;

            private string response_Description_ArField;

            private string response_Description_EnField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("Response_Code")]
            public byte Response_Code1
            {
                get
                {
                    return this.response_Code1Field;
                }
                set
                {
                    this.response_Code1Field = value;
                }
            }

            /// <remarks/>
            public string Response_Description_Ar
            {
                get
                {
                    return this.response_Description_ArField;
                }
                set
                {
                    this.response_Description_ArField = value;
                }
            }

            /// <remarks/>
            public string Response_Description_En
            {
                get
                {
                    return this.response_Description_EnField;
                }
                set
                {
                    this.response_Description_EnField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class Marriage
        {

            private MarriageItem[] itemField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("item")]
            public MarriageItem[] item
            {
                get
                {
                    return this.itemField;
                }
                set
                {
                    this.itemField = value;
                }
            }
        }

        public class oAuthTokenGeneration
        {
            public TokenDetails GenerateToken(string uri, string grant_type, string client_id, string client_secret)
            {
                TokenDetails objToken = new TokenDetails();
                StringBuilder tokenUri = new StringBuilder();
                tokenUri.Append(uri);
                tokenUri.AppendFormat("?grant_type=" + grant_type + "&client_id=" + client_id + "&client_secret=" + client_secret + "&scope=" + "crm");
                String responseBody;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                try
                {
                    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(tokenUri.ToString());
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    StreamReader responseStream = new StreamReader(response.GetResponseStream());
                    responseBody = responseStream.ReadToEnd();
                    objToken = JsonConvert.DeserializeObject<TokenDetails>(responseBody);
                    return objToken;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }

    }

}