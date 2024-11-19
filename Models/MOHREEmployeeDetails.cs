using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MOHREEmployeeDetails
    {
        public class MOHRERequest
        {
            public string EmiratesID { get; set; }
        }
        public class MOHREResponse
        {
            public string emiratesIDNumber { get; set; }
            public string employeeNameEnglish { get; set; }
            public string employeeNameArabic { get; set; }
            public string companyCode { get; set; }
            public string companyNameEnglish { get; set; }
            public string companyNameArabic { get; set; }
            public string salary { get; set; }
            public string contractStartDate { get; set; }
            public string contractEndDate { get; set; }
            public string SuccessFlag { get; set; }
            public string ErrorFlag { get; set; }
            public string ErrorDescEnglish { get; set; }
            public string ErrorDescArabic { get; set; }
        }
    }
}