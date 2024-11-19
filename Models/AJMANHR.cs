using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class AJMANHR
    {

        public class AJMANHRRequest
        {
            public string EmiratesID { get; set; }
        }
        public class AJMANHRResponse
        {
            public string NATIONAL_IDENTIFIER { get; set; }
            public string EMPLOYEE_NUMBER { get; set; }
            public string EMPLOYEE_NAME { get; set; }
            public string DEPARTEMENT { get; set; }
            public string NATIONALITY { get; set; }
            public string HIRE_DATE { get; set; }
            public string CF_BASIC_SALARY { get; set; }
            public string CF_ACTUAL_NEW_COMPLEMENTARY { get; set; }
            public string CF_COMPLEMENTARY_SAL { get; set; }
            public string CF_TRANSPORT_ALLOWANCE { get; set; }
            public string CF_CHILD_LOCAL { get; set; }
            public string CF_WORK_NATURE_ALLOWANCE { get; set; }
            public string CF_SCIENTIFIC_DEGREE { get; set; }
            public string CF_SPECIAL_ALLOWANCE { get; set; }
            public string CF_PD_PHONE_CALLS_ALLOWANCE { get; set; }
            public string CF_EMPLOYEE_CUT_SALARY { get; set; }
            public string CF_WORKER_CUT_SALARY { get; set; }
            public string CF_HOUSING_ALLO { get; set; }
            public string CF_SPECIAL_LOCAL { get; set; }
            public string CF_COST_OF_LIFE { get; set; }
            public string CF_EXCEPTIONAL_ALLOWANCE { get; set; }
            public string CF_TECHNICAL_ALLOWANCE { get; set; }
            public string CF_NE_WORK_NATURE { get; set; }
            public string CF_TOTAL_ADDITIONAL_EARNING { get; set; }
            public string CF_EMPLOYER_SHARE { get; set; }
            public string CF_EMPLOYEE_SHARE { get; set; }
        }
    }

    public class InputParameters
    {
        public string P_NATIONAL_IDENTIFIER { get; set; }
    }

    public class RootObject
    {
        public InputParameters InputParameters { get; set; }
    }

}