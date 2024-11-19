using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MOJCaseDetailsModel
    {
        public class CaseDetails
        {
            public string EMIRATE { get; set; }
            public string COURT { get; set; }
            public string CASE_NUMBER { get; set; }
            public string YEAR { get; set; }
            public string CASE_STATUS { get; set; }
            public string COURT_TYPE { get; set; }
            public string PROCEEDING_TYPE { get; set; }
            public string SUB_PROCEEDING_TYPE { get; set; }
            public string FILING_PARTY_TYPE { get; set; }
            public string FILING_PARTY_NAME { get; set; }
            public string DATE_CASE_OPENED { get; set; }
            public string CLAIM_AMOUNT { get; set; }
            public string FEE { get; set; }
            public string FEE_PAYMENT_STATUS { get; set; }
            public PLAINTIFF Plaintiff { get; set; }
            public bool Status { get; set; }


        }
        public class PLAINTIFF
        {
            public string PLAINTIFF_NAME { get; set; }
            public string PLAINTIFF_NATIONALITY { get; set; }
            public string PLAINTIFF_E_ID { get; set; }
            public string PLAINTIFF_MOBILE { get; set; }
            public string PLAINTIFF_EMAIL { get; set; }
            public string Type { get; set; }

        }
        public class DEFENDANT
        {
            public string DEFENDANT_NAME { get; set; }
            public string DEFENDANT_NATIONALITY { get; set; }
            public string DEFENDANT_E_ID { get; set; }
            public string DEFENDANT_MOBILE { get; set; }
            public string DEFENDANT_EMAIL { get; set; }
        }
    }
}