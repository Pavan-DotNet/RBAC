using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class ADGDetails
    {
        public class OutputParameters
        {
            [JsonProperty("@xmlns")]
            public string xmlns { get; set; }

            [JsonProperty("@xmlns:xsi")]
            public string xmlnsxsi { get; set; }
            public string P_EMPLOYEE_NUMBER { get; set; }
            public string P_DEPARTMENT_AR { get; set; }
            public string P_GRADE { get; set; }
            public string P_JOB_NAME { get; set; }
            public string P_POSITION_NAME { get; set; }
            public string P_NATIONALITY_AR { get; set; }
            public string P_NATIONALITY_EN { get; set; }
            public string P_TOTAL_SLARY { get; set; }
            public string P_SOCIAL_INSURANCE { get; set; }
            public string P_CODE { get; set; }
            public string P_OUT_MSG { get; set; }
        }

        public class Root
        {
            public OutputParameters OutputParameters { get; set; }
        }
    }
}