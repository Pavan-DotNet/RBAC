using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class SHJPRISONDATA
    {
        public class InquiryRequest
        {
            public string IDType { get; set; }
            public string IDNumber { get; set; }
            public InmateDetails InmateDetails { get; set; }
            public string ChannelType { get; set; }
        }

        public class InmateDetails
        {
            public string InmateName { get; set; }
        }


        public class InquirySubmitResponse
        {
            public string Success { get; set; }
            public string ResponseEN { get; set; }
            public string ResponseAR { get; set; }
            public string Data { get; set; }
            public string ExceptionMessage { get; set; }
            public string ErrorIdentifier { get; set; }
            public string ErrorType { get; set; }
        }

        public class InquiryStatusResponse
        {
            public string Success { get; set; }
            public string ResponseEN { get; set; }
            public string ResponseAR { get; set; }
            public Data Data { get; set; }
            public string ExceptionMessage { get; set; }
            public string ErrorIdentifier { get; set; }
            public string ErrorType { get; set; }
        }

        public class Data
        {
            public string Id { get; set; }
            public string ReferenceNumber { get; set; }
            public string IDType { get; set; }
            public string IDNumber { get; set; }
            public string InmateName { get; set; }
            public string Status { get; set; }
            public string ReasonforRejection { get; set; }
            public string InmateStatusType { get; set; }
            public string CaseType { get; set; }
            public string DateofEntry { get; set; }
            public string DurationofSentence { get; set; }
            public string ExpectedReleaseDate { get; set; }
            public string ReleasedDate { get; set; }
            public string TransferredDate { get; set; }
            public string TransferredTo { get; set; }
            public string ReasonForTransfer { get; set; }
        }


        public class SHJPrResponse
        {
            public string TransactionId { get; set; }
            public string EmiratesId { get; set; }
            public string InMateName { get; set; }
            public string InsertDate { get; set; }
  
        }

    }
}