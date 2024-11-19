using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MOIHomeArresPrisonersDetails
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Content
        {
            public string ArabicName { get; set; }
            public string EnglishName { get; set; }
            public int MonitorValue { get; set; }
            public string MonitorType { get; set; }
            public string RefCaseType { get; set; }
            public string StartDate { get; set; }
            public string EndDate { get; set; }
            public string ActualEndDate { get; set; }
        }

        public class Root
        {
            public Content content { get; set; }
        }


    }
}