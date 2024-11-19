using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class ADAFSADetails
    {
        public class ADAFSADetailsRequestParams
        {
            public string EmiratesID { get; set; }

        }
        public class ADAFSAResponse
        {
            public class BeneficiaryResponse
            {
                public string AgricultureCardNo { get; set; }
                public string OwnerNameAR { get; set; }
                public string Status { get; set; }
                public string PhoneNumber { get; set; }
                public string PhoneNumber2 { get; set; }
                public string EmiratesId { get; set; }
                public string EmiratesExpiryDate { get; set; }
                public string BankAccountName { get; set; }
                public string BankAccountNumber { get; set; }
                public string TransactionTypeAR { get; set; }
                public string TransferTypeAR { get; set; }
                public string BankName { get; set; }
                public string BankBranch { get; set; }
                public string LastPaidMonth { get; set; }
                public string LastPaidAmount { get; set; }
            }


        }
    }
}