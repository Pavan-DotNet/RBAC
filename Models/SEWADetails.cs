using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SEWADetails
    {
        public class SEWARequest
        {
            public string EmiratesId { get; set; }
            public string LSocialSecurityNo { get; set; }
            public string FSocialSecurityNo { get; set; }
        }

        public class SEWAResponse
        {
            public string addresS2 { get; set; }
            public string addresS1 { get; set; }
            public string areA_DESC { get; set; }
            public string sitE_OFFICE { get; set; }
            public string tenanT_NAME { get; set; }
            public string owneR_NAME { get; set; }
            public string conS_NUM { get; set; }
            public string preM_TYPE { get; set; }
            public string accT_ID { get; set; }
            public string addresS3 { get; set; }
            public string discounT_PERCENTAGE { get; set; }
            public string discounT_START_DT { get; set; }
            public string tenanT_EMAIL { get; set; }
            public string tenanT_MOBILE { get; set; }
            public string tenanT_EMIRATES_ID { get; set; }
            public string tenanT_SOCIAL_SEC_NUM { get; set; }
            public string tenanT_SHJ_SOCIAL_SEC_NUM { get; set; }
            public string tenanT_SPL_NEED { get; set; }
            public string owneR_EMAIL { get; set; }
            public string owneR_MOBILE { get; set; }
            public string owneR_EMIRATES_ID { get; set; }
            public string owneR_SOCIAL_SEC_NUM { get; set; }
            public string owneR_SHJ_SOCIAL_SEC_NUM { get; set; }
            public string errorCode { get; set; }
            public string responseCode { get; set; }
            public string responseMessage { get; set; }
        }
    }
}