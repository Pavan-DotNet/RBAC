using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{

    public class MOJRequestModel
    {
        public string EmiratesId { get; set; }
    }

    public class MOJCaseDetails
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
        public float CLAIM_AMOUNT { get; set; }
        public List<MOJPLAINTIFF> Plaintiff { get; set; }
        public List<MOJDEFENDANT> Defendant { get; set; }
        public float FEE { get; set; }
        public string FEE_PAYMENT_STATUS { get; set; }

    }

    public class MOJPLAINTIFF
    {
        public string PLAINTIFF_NAME { get; set; }
        public string PLAINTIFF_NATIONALITY { get; set; }
        public string PLAINTIFF_E_ID { get; set; }
        public string PLAINTIFF_MOBILE { get; set; }
        public string PLAINTIFF_EMAIL { get; set; }
    }

    public class MOJDEFENDANT
    {
        public string DEFENDANT_NAME { get; set; }
        public string DEFENDANT_NATIONALITY { get; set; }
        public string DEFENDANT_E_ID { get; set; }
        public string DEFENDANT_MOBILE { get; set; }
        public string DEFENDANT_EMAIL { get; set; }
    }
}