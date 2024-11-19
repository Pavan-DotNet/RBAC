using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class DewaDetailsModel
    {
        public class Address
        {
            public string houseNumber { get; set; }
            public string street { get; set; }
            public string district { get; set; }
            public string postalCode { get; set; }
            public string city { get; set; }
        }

        public class Body
        {
            public KeyData keyData { get; set; }
            public PremiseDetailsResp premiseDetailsResp { get; set; }
            public Message message { get; set; }
        }

        public class GetPremiseDetailsResponse
        {
            public Header Header { get; set; }
            public Body Body { get; set; }
        }

        public class Header
        {
            public string serviceName { get; set; }
            public string serviceVersion { get; set; }
            public string serviceLang { get; set; }
            public string hashvalue { get; set; }
        }

        public class KeyData
        {
            public string transactionRefNo { get; set; }
            public DateTime timeStamp { get; set; }
        }

        public class Message
        {
            public string type { get; set; }
            public string code { get; set; }
            public string description { get; set; }
        }

        public class PremiseDetailsResp
        {
            public List<Record> record { get; set; }
        }

        public class Record
        {
      
            public int Id { get; set; }
            public string inputIdType { get; set; }
            public string inputIdNumber { get; set; }
            public string premiseNo { get; set; }
            public string premiseType { get; set; }
            public string contractAccount { get; set; }
            public string contractAccountType { get; set; }
            public string ownerName { get; set; }
            public string uaeNational { get; set; }
            public string moveInDate { get; set; }
            public string moveOutDate { get; set; }
            public string makaniNumber { get; set; }
            public string eidNumber { get; set; }
            public string socialBenifit { get; set; }
            public string communityNumber { get; set; }
            public string billingCycle { get; set; }
            public string recInflationAllowance { get; set; }
            public Address Address { get; set; }
            public DateTime InsertDate { get; set; } = DateTime.Now;

        }

        public class Root
        {
            public GetPremiseDetailsResponse GetPremiseDetailsResponse { get; set; }
        }
    }

}