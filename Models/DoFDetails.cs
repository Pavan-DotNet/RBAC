using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class DoFDetails
    {
        public class DoFRequest
        {
            public string EmiratesID { get; set; }
        }

        public class DoFResponse
        {
            public string PENSIONER_NAME_AR { get; set; }
            public string PENSIONER_NAME_US { get; set; }
            public string PENSION_START_DATE { get; set; }
            public string PENSION_AMOUNT { get; set; }
            public string PENSION_ID { get; set; }
            public string PENSIONER_DEATH { get; set; }
            public string EMIRATES_ID { get; set; }
        }
       
    }
}