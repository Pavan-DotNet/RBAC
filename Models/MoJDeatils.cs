using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MoJDeatils
    {
        public class MoJDeatilsRequest
        {
            public string EmiratesID { get; set; }
            public string Name { get; set; }
            public string DOB { get; set; }

        }
        public class MoJDeatilsResponse
        {
            public string AppNo { get; set; }
            public string ContractNo { get; set; }
            public DateTime ContractDate { get; set; }
            public string Husband_ID { get; set; }
            public DateTime Husband_Dob { get; set; }
            public string Husband_NameAr { get; set; }
            public string Wife_ID { get; set; }
            public DateTime Wife_Dob { get; set; }
            public string Wife_NameAr { get; set; }
            public string Sons_Num { get; set; }
            public string Husband_BoysNo { get; set; }
            public string Husband_GirlsNo { get; set; }
            public string Wife_BoysNo { get; set; }
            public string Wife_GirlsNo { get; set; }
        }
    }
}