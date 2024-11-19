using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class RAKDivorceDetailsModel
    {
        public class RAKCourtsDivorceEnvelope
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

                private MT_DivorceDetails_Res mT_DivorceDetails_ResField;

                /// <remarks/>
                [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://ega.rak.ae/xi/MarriageandDivorceInquiry")]
                public MT_DivorceDetails_Res MT_DivorceDetails_Res
                {
                    get
                    {
                        return this.mT_DivorceDetails_ResField;
                    }
                    set
                    {
                        this.mT_DivorceDetails_ResField = value;
                    }
                }
            }

            /// <remarks/>
            [System.SerializableAttribute()]
            [System.ComponentModel.DesignerCategoryAttribute("code")]
            [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://ega.rak.ae/xi/MarriageandDivorceInquiry")]
            [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://ega.rak.ae/xi/MarriageandDivorceInquiry", IsNullable = false)]
            public partial class MT_DivorceDetails_Res
            {

                private DivorceItem[] divorceField;

                private Response_Code response_CodeField;

                /// <remarks/>
                [System.Xml.Serialization.XmlArrayAttribute(Namespace = "")]
                [System.Xml.Serialization.XmlArrayItemAttribute("item", IsNullable = false)]
                public DivorceItem[] Divorce
                {
                    get
                    {
                        return this.divorceField;
                    }
                    set
                    {
                        this.divorceField = value;
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
            public partial class DivorceItem
            {

                private uint zTransactionIdField;
                private string zDivorceContractNumField;
                private string zDivorceContractYearField;
                private string zDivorceTypeField;
                private string zContractDateField;
                private string zContractDateHijriField;
                private string zActualDivorceDateField;
                private string zDivorcerNameArField;
                private string zDivorcerEidField;
                private string zDivorcerPassportNumField;
                private string zDivorcerPassportCouField;
                private string zDivorcerKhulasitQaidField;
                private string zDivorcerDOBField;
                private string zDivorcerNatField;
                private string zDivorcerRelField;
                private string zDivorceeNameArField;
                private string zDivorceeEidField;
                private string zDivorceePassportNumField;
                private string zDivorceePassportCouField;
                private object zDivorceeKhulasitQaidField;
                private string zDivorceeDOBField;
                private string zDivorceeNatField;
                private string zDivorceeRelField;

                /// <remarks/>
                public uint ZTransactionId
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
                public string ZDivorceContractNum
                {
                    get
                    {
                        return this.zDivorceContractNumField;
                    }
                    set
                    {
                        this.zDivorceContractNumField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorceContractYear
                {
                    get
                    {
                        return this.zDivorceContractYearField;
                    }
                    set
                    {
                        this.zDivorceContractYearField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorceType
                {
                    get
                    {
                        return this.zDivorceTypeField;
                    }
                    set
                    {
                        this.zDivorceTypeField = value;
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
                public string ZActualDivorceDate
                {
                    get
                    {
                        return this.zActualDivorceDateField;
                    }
                    set
                    {
                        this.zActualDivorceDateField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorcerNameAr
                {
                    get
                    {
                        return this.zDivorcerNameArField;
                    }
                    set
                    {
                        this.zDivorcerNameArField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorcerEid
                {
                    get
                    {
                        return this.zDivorcerEidField;
                    }
                    set
                    {
                        this.zDivorcerEidField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorcerPassportNum
                {
                    get
                    {
                        return this.zDivorcerPassportNumField;
                    }
                    set
                    {
                        this.zDivorcerPassportNumField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorcerPassportCou
                {
                    get
                    {
                        return this.zDivorcerPassportCouField;
                    }
                    set
                    {
                        this.zDivorcerPassportCouField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorcerKhulasitQaid
                {
                    get
                    {
                        return this.zDivorcerKhulasitQaidField;
                    }
                    set
                    {
                        this.zDivorcerKhulasitQaidField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorcerDOB
                {
                    get
                    {
                        return this.zDivorcerDOBField;
                    }
                    set
                    {
                        this.zDivorcerDOBField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorcerNat
                {
                    get
                    {
                        return this.zDivorcerNatField;
                    }
                    set
                    {
                        this.zDivorcerNatField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorcerRel
                {
                    get
                    {
                        return this.zDivorcerRelField;
                    }
                    set
                    {
                        this.zDivorcerRelField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorceeNameAr
                {
                    get
                    {
                        return this.zDivorceeNameArField;
                    }
                    set
                    {
                        this.zDivorceeNameArField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorceeEid
                {
                    get
                    {
                        return this.zDivorceeEidField;
                    }
                    set
                    {
                        this.zDivorceeEidField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorceePassportNum
                {
                    get
                    {
                        return this.zDivorceePassportNumField;
                    }
                    set
                    {
                        this.zDivorceePassportNumField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorceePassportCou
                {
                    get
                    {
                        return this.zDivorceePassportCouField;
                    }
                    set
                    {
                        this.zDivorceePassportCouField = value;
                    }
                }

                /// <remarks/>
                public object ZDivorceeKhulasitQaid
                {
                    get
                    {
                        return this.zDivorceeKhulasitQaidField;
                    }
                    set
                    {
                        this.zDivorceeKhulasitQaidField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorceeDOB
                {
                    get
                    {
                        return this.zDivorceeDOBField;
                    }
                    set
                    {
                        this.zDivorceeDOBField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorceeNat
                {
                    get
                    {
                        return this.zDivorceeNatField;
                    }
                    set
                    {
                        this.zDivorceeNatField = value;
                    }
                }

                /// <remarks/>
                public string ZDivorceeRel
                {
                    get
                    {
                        return this.zDivorceeRelField;
                    }
                    set
                    {
                        this.zDivorceeRelField = value;
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
            public partial class Divorce
            {

                private DivorceItem[] itemField;

                /// <remarks/>
                [System.Xml.Serialization.XmlElementAttribute("item")]
                public DivorceItem[] item
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

        }
    }
}