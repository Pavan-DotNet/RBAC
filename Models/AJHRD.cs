using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class AJHRD
    {
        public class AJHRDDetailsRequestParams
        {
            public string EmiratesId { get; set; }
        }

        public class OutputParameters
        {
            public string Xmlns { get; set; }
            public string XmlnsXsi { get; set; }
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
            public object P_OUT_MSG { get; set; }
        }
        public class Rooot
        {
            public OutputParameters OutputParameters { get; set; }
        }
        public class InputParameters
        {
            public string P_NATIONAL_IDENTIFIER { get; set; }
        }

        public class RootInput
        {
            public InputParameters InputParameters { get; set; }
        }

        public class mocd_disability
        {
            public string PK { get; set; }
            public string TYPE { get; set; }
            public string ORGANIZATION { get; set; }
            public string FATHER_NAME_ENGLISH { get; set; }
            public string NATIONALITY { get; set; }
            public string FIRST_NAME_ARABIC { get; set; }
            public string OUT_RESIDENCE_EXPIRY_DATE { get; set; }
            public string CENTER { get; set; }
            public string FATHER_NAME_ARABIC { get; set; }
            public string GRANDFATHER_ENGLISH { get; set; }
            public string MARRITAL_STATUS { get; set; }
            public string COMPANY { get; set; }
            public string QUALIFICATION { get; set; }
            public string IDENTIFICATION_NO_ { get; set; }
            public string FAMILY_NAME_ARABIC { get; set; }
            public string WORKING_STATUS { get; set; }
            public string INSTITUTES { get; set; }
            public string OUT_DATE_OF_BIRTH { get; set; }
            public string WORKING_TYPE { get; set; }
            public string GENDER { get; set; }
            public string ACCOMMODATION_TYPE { get; set; }
            public string UIDE { get; set; }
            public string STUDENT { get; set; }
            public string FAMILY_NAME_ENGLISH { get; set; }
            public string FIRST_NAME_ENGLISH { get; set; }
            public string GRANDFATHER_ARABIC { get; set; }
            public string TELEPHONE_NO { get; set; }
            public string OTHER_MOBILE_NO { get; set; }
            public string PO_BOX { get; set; }
            public string ADDRESS { get; set; }
            public string MAKANI_NO { get; set; }
            public string EMAIL { get; set; }
            public string MOBILE_NO { get; set; }
            public string EMIRATE { get; set; }
            public string Y_COORDINATE { get; set; }
            public string X_COORDINATE { get; set; }
            public string DIAGNOSIS_INFORMATION1 { get; set; }
            public string REPORT_DATE { get; set; }
            public string DISABILITY_LEVEL { get; set; }
            public string SPECIALITY { get; set; }
            public string NEED_SUPPORTER { get; set; }
            public string SUPPORTING_EQUIPMENTS { get; set; }
            public string DIAGNOSIS_AUTHORITY { get; set; }
            public string DISABILITIES_TYPE { get; set; }
            public string REPORT_ISSUED_BY { get; set; }
            public string CAN_LIVE_ALONE { get; set; }
            public string CARD_EXPIRY_DATE { get; set; }
            public string OUT_CARD_ISSUE_DATE { get; set; }
            public string DISABLED_CARD_NO { get; set; }
            //public string LOAD_DATE { get; set; }
            public string STATUS { get; set; }


        }
    }
}