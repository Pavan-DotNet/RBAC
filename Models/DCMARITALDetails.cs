using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class DCMARITALDetails
    {
        public class DCMARITALRequest
        {
            public string MSType { get; set; }
            public string DocumentType { get; set; }
            public string DocumetIssuerAuthority { get; set; }
            public string DocumentNumber { get; set; }
            public string DocumentYear { get; set; }
            public string EmiratesIdHusband { get; set; }
            public string EmiratesIdWife { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string HusbandName { get; set; }
            public string WifeName { get; set; }
        }
        public class DCMARITALResponse
        {

            public string DocumentType { get; set; }
            public string CertifcateRef { get; set; }
            public string GregorianDate { get; set; }
            public string Party1Name { get; set; }
            public string Party1UID { get; set; }
            public string Party1Nationality { get; set; }
            public string Party1BirthDate { get; set; }
            public string Party2Name { get; set; }
            public string Party2UID { get; set; }
            public string Party2Nationality { get; set; }
            public string Party2BirthDate { get; set; }
            public string BinaryFile { get; set; }

        }

    }
}