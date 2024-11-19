using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class ShrajahMunciplalityDetails
    {
        public class Sharjah_GSB_MunRequestParams
        {
            public string EmiratesId { get; set; }
        }
        public class Rooot
        {
            public List<data> data { get; set; }
        }
        public class data
        {
            public string EmiratesId { get; set; }
            public string ResponseCode { get; set; }
            public string ResponseMessage { get; set; }
            public string contracT_NO { get; set; }
            public string conT_TYPE { get; set; }
            public string flaT_DESC { get; set; }
            public string tenanT_NAME { get; set; }
            public string passporT_NO { get; set; }
            public string conT_START_DATE { get; set; }
            public string conT_END_DATE { get; set; }
            public string area { get; set; }
            public string zonE_CODE { get; set; }
            public string lanD_NO { get; set; }
            public string status { get; set; }
            public string uaE_NATIONAL_ID { get; set; }
            public string nationaL_NO { get; set; }
        }
    }
}
