using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class LULLUInquiryDetails
    {
        // Root myDeserializedClass = JsonConvert.Deserializestring<Root>(myJsonResponse);
        public class Root
        {
            public string TransactionId { get; set; }
            public string TrackData { get; set; }
            public string CardNumber { get; set; }
            public string CardPIN { get; set; }
            public string InvoiceNumber { get; set; }
            public string Amount { get; set; }
            public string BillAmount { get; set; }
            public string OriginalInvoiceNumber { get; set; }
            public string OriginalTransactionId { get; set; }
            public string OriginalBatchNumber { get; set; }
            public string OriginalApprovalCode { get; set; }
            public string OriginalAmount { get; set; }
            public string ApprovalCode { get; set; }
            public string TotalPreAuthAmount { get; set; }
            public string TotalRedeemedAmount { get; set; }
            public string TotalReloadedAmount { get; set; }
            public string AddonCardNumber { get; set; }
            public string AddonCardTrackData { get; set; }
            public string TransferCardNumber { get; set; }
            public string MerchantName { get; set; }
            public string AdjustmentAmount { get; set; }
            public string CardExpiry { get; set; }
            public string OriginalCardNumber { get; set; }
            public string OriginalCardPin { get; set; }
            public string CardProgramID { get; set; }
            public string CorporateName { get; set; }
            public string Notes { get; set; }
            public string SettlementDate { get; set; }
            public string Expiry { get; set; }
            public string PurchaseOrderNumber { get; set; }
            public string PurchaseOrderValue { get; set; }
            public string DiscountPercentage { get; set; }
            public string DiscountAmount { get; set; }
            public string PaymentMode { get; set; }
            public string PaymentDetails { get; set; }
            public bool BulkType { get; set; }
            public string ExternalCorporateId { get; set; }
            public string ExternalCardNumber { get; set; }
            public string ResponseCode { get; set; }
            public string ResponseMessage { get; set; }
            public string MerchantOutletName { get; set; }
            public string MerchantOutletAddress1 { get; set; }
            public string MerchantOutletAddress2 { get; set; }
            public string MerchantOutletCity { get; set; }
            public string MerchantOutletState { get; set; }
            public string MerchantOutletPinCode { get; set; }
            public string MerchantOutletPhone { get; set; }
            public string MaskCard { get; set; }
            public string PrstringMerchantCopy { get; set; }
            public string InvoiceNumberMandatory { get; set; }
            public string NumericUserPwd { get; set; }
            public string stringegerAmount { get; set; }
            public string Culture { get; set; }
            public string CurrencySymbol { get; set; }
            public string CurrencyPosition { get; set; }
            public string CurrencyDecimalDigits { get; set; }
            public string DisplayUnitForPostrings { get; set; }
            public string ReceiptFooterLine1 { get; set; }
            public string ReceiptFooterLine2 { get; set; }
            public string ReceiptFooterLine3 { get; set; }
            public string ReceiptFooterLine4 { get; set; }
            public bool Result { get; set; }
            public string TransferCardExpiry { get; set; }
            public string TransferCardBalance { get; set; }
            public string CardStatusId { get; set; }
            public string CardStatus { get; set; }
            public string CardCurrencySymbol { get; set; }
            public string CurrencyCode { get; set; }
            public string ActivationDate { get; set; }
            public List<SVRecentTransaction> SVRecentTransactions { get; set; }
            public string CardType { get; set; }
            public string TransferCardTrackData { get; set; }
            public string TransferCardPin { get; set; }
            public string CumulativeAmountSpent { get; set; }
            public string CurrencyConversionRate { get; set; }
            public string CurrencyConvertedAmount { get; set; }
            public string CardHolderName { get; set; }
            public string StoredValueUnitID { get; set; }
            public string XactionAmountConvertedValue { get; set; }
            public string StoredValueConvertedAmount { get; set; }
            public string PromotionalValue { get; set; }
            public string EarnedValue { get; set; }
            public string TransactionAmount { get; set; }
            public string PreviousBalance { get; set; }
            public string UpgradedCardProgramGroupName { get; set; }
            public string NewBatchNumber { get; set; }
            public string OriginalActivationAmount { get; set; }
            public string CardCreationType { get; set; }
            public string ErrorCode { get; set; }
            public string ErrorDescription { get; set; }
            public string CardProgramGroupType { get; set; }
            public string ActivationCode { get; set; }
            public string ActivationURL { get; set; }
            public string MerchantID { get; set; }
            public string ReloadableAmount { get; set; }
            public string Barcode { get; set; }
            public string ThemeId { get; set; }
            public string CurrentBatchNumber { get; set; }
            public string MID { get; set; }
            public string Customer { get; set; }
            public string TransactionType { get; set; }
        }

        public class SVRecentTransaction
        {
            public string CardNumber { get; set; }
            public string TransactionDate { get; set; }
            public string TransactionDateAtServer { get; set; }
            public string OutletName { get; set; }
            public string OutletCode { get; set; }
            public string InvoiceNumber { get; set; }
            public string TransactionType { get; set; }
            public string TransactionAmount { get; set; }
            public string CardBalance { get; set; }
            public string Loyalty { get; set; }
            public string Notes { get; set; }
        }


    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class LULLUInquiryDetailsRequest
    {
        public string TransactionId { get; set; }
        public string CardNumber { get; set; }
        public string DateAtClient { get; set; }
        //public string CardPIN { get; set; }
    }
}