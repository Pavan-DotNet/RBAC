using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class InternalInflationDetails
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Content
        {
            public string emiratesId { get; set; }
            public string caseNumber { get; set; }
            public string nationality { get; set; }
            public string caseId { get; set; }
            public string nationalityId { get; set; }
            public string errorMessage { get; set; }
            public string nameArabic { get; set; }
            public DateTime dateofBirth { get; set; }
            public string caseStatusAr { get; set; }
            public string serviceRequestType { get; set; }
            public string caseOriginCode { get; set; }
            public string ownerId { get; set; }
            public string socialSecurityProcedures { get; set; }
            public double mocd_adnocfuelamount { get; set; }
            public List<string> ami_allowancetypes { get; set; }
            public string ami_typeoffuelbenefit { get; set; }
            public double mocd_foodbenefitamount { get; set; }
            public string id { get; set; }
        }

        public class Root
        {
            public List<Content> content { get; set; }
            public int code { get; set; }
            public string responseTitle { get; set; }
            public string responseDescriptionEn { get; set; }
            public string responseDescriptionAr { get; set; }
            public bool successful { get; set; }
            public object properties { get; set; }
            public object errorAdditionalInfo { get; set; }
        }


    }
}