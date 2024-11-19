using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MoIDetails
    {
        public class MoIDetailsRequestParams
        {
            public string EmiratesID { get; set; }
        }
        public class MoIDetailsResponseParams
        {
            public string EmiratesId { get; set; }
            public bool? isPersonHasBeenImprisoned { get; set; }
            public long? personCertificateStatusId { get; set; }
            public string personCertificateStatusEn { get; set; }
            public string personCertificateStatusAr { get; set; }
            public bool? isPersonHasTreatment { get; set; }
            public bool? isPersonExitCountry { get; set; }
        }
    }
}