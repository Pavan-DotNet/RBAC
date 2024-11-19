using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MOIPrisonersDetails
    {
      
            public class MIOFetailsRequestParams
            {
                public string EmiratesId { get; set; }

            public string UnifiedNumber { get; set; }


        }

        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Content
            {
                public string EmiratesId { get; set; }
                public string ArabicFullName { get; set; }
                public string EnglishFullName { get; set; }
                public string EntryDate { get; set; }
                public string ReleaseDate { get; set; }
                public string ExpectedDate { get; set; }
                public string Description { get; set; }
                public string CRF_File_No { get; set; }
                public string FileCode { get; set; }
            }

            public class Root
            {
                public Content content { get; set; }
            }


        }

    }
