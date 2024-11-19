using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class AADCAccountDetails
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

        public class Rooot
        {
      
            public int Id { get; set; }
            public string EID { get; set; }

            public string inputIdNumber { get; set; }
            public string ACC_Status { get; set; }
            public string ACC_Type { get; set; }
            public string UAE_National { get; set; }
            public string Bill_Cycle { get; set; }
            public string receivingInflationallowances { get; set; }
            public string SocialCardallowance { get; set; }
            public string statuscode { get; set; }
            public string statusMessage { get; set; }

            public DateTime InsertDate { get; set; } = DateTime.Now;
        }


    }

}