using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class ZFDetails
    {
        public class ZFDetailsRequest
        {

            public string EmiratesID { get; set; }
        }

        public class ZFDetailsResponse
        {
            public string BeneficiaryName { get; set; }
            public string CaseTypeAr { get; set; }
            public string CaseTypeEn { get; set; }
            public string ClassificationAr { get; set; }
            public string ClassificationEn { get; set; }
            public string ContainerAr { get; set; }
            public string ContainerEn { get; set; }
            public string FamilyNo { get; set; }
            public string FileNo { get; set; }
            public string FileStatusAr { get; set; }
            public string FileStatusEn { get; set; }
            public string HelpAmount { get; set; }
            public string HelpTypeAr { get; set; }
            public string HelpTypeEn { get; set; }
            public string IdentityNumber { get; set; }
            public string MobileNo { get; set; }
            public string NameAr { get; set; }
            public string NameEn { get; set; }
            public string NoOfApplications { get; set; }
            public string NoOfInstallment { get; set; }
            public string ApplicationDate { get; set; } 
            public string ApprovalDate { get; set; }
        }
    }
}