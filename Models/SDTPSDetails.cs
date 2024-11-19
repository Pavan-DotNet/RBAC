using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SDTPSDetails
    {
        
        public class BranchList
        {
            public int branchId { get; set; }
            public string branchNameAr { get; set; }
            public string branchNameEn { get; set; }
        }
        public class SDTPSDetailsRequestParam
        {
            public string EmiratesId { get; set; }
            public string BranchCode { get; set; }
            public string BranchName { get; set; }
            public string ReceiverId { get; set; }
            public string UserId { get; set; }

            public string BeneficiaryName { get; set; }
        }

        public class SDTPSDetailsResponseParam
        {
            public string Id { get; set; }
            public string EmiratesId { get; set; }
            public string TransactionNo { get; set; }
            public string BranchName { get; set; }

        }
        public class SDTPSTransactionDetailsResponse
        {
            public string TransactionNumber { get; set; }
            public string UserName { get; set; }
            public string UserId { get; set; }
            public string BeneficiaryName { get; set; }
            public string BeneficiaryId { get; set; }
            public string InsertDate { get; set; }
            public string Status { get; set; }
            public string Base64 { get; set; }

        }
        

    }
  
  
}