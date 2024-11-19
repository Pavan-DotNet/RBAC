using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class DPDeathQueryDeatils
    {
        public class DPDeathQueryDeatilsRequest
        {           
            public string EmiratesID { get; set; }           
        }

        public class DPDeathQueryDeatilsResponse
        {
            public string EMIRATES_ID { get; set; }
            public string NAME_ARABIC { get; set; }
            public string NAME_ENGLISH { get; set; }
            public string UNIFIED_NO { get; set; }
            public string NATIONALITY_CODE { get; set; }
            public string NATIONALITY_DESC { get; set; }
            public string ENTITY_NAME_AR { get; set; }
            public string DOB { get; set; }
            public Int32 GENDER { get; set; }
            public string GENDER_DESC { get; set; }
            public string MOTHER_NAME_AR { get; set; }
            public string CASE_DATE { get; set; }
            public string UserType { get; set; }
            public string PersonId { get; set; }
        }
               
    }
}