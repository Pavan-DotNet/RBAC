using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class FAHRDetails
    {
        public class FAHRRequestParams
        {
            public string EmiratesId { get; set; }
            public string EmployeeName { get; set; }
            public string DOB { get; set; }
            public string FAMILYNO { get; set; }
            public string EMIRATENO { get; set; }

        }

        public class FAHRResponseParams
        {
            public string EMIRATES_ID { get; set; }
            public string EMPLOYEE_NUMBER { get; set; }
            public string EMPLOYEE_NAME { get; set; }
            public string EMPLOYEE_NAME_EN { get; set; }
            public string ENTITY_CODE { get; set; }
            public string ENTITY_NAME_EN { get; set; }
            public string ENTITY_NAME_AR { get; set; }
            public string HIRE_DATE { get; set; }
            public Int32 MONTHLY_SALARY { get; set; }
            public string END_OF_SERVICE_DATE { get; set; }
            public string END_OF_SERVICE_REASON { get; set; }
            public string PERSON_TYPE_AR { get; set; }
            public string PERSON_TYPE_EN { get; set; }


        }
    }
}