using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class AJMStakeHoldersDetails
    {
        public class AJMStakeHoldersRequestParams
        {
            public string EmiratesId { get; set; }
            public string Passport { get; set; }
            public string Code { get; set; }


        }
        public class AJMStakeHolderDetailsModel
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            public class ErrorTable
            {
                public string ErrorCode { get; set; }
                public string ErrorMessage { get; set; }
                public string ErrorEx { get; set; }
            }

            public class GetStakeholderInfoResult
            {
                public Stakeholders Stakeholders { get; set; }
                public ErrorTable ErrorTable { get; set; }
            }

            public class License
            {
                public string LicenseId { get; set; }
                public string LicenseNumber { get; set; }
                public string TradeNameAR { get; set; }
                public string TradeNameEN { get; set; }
                public DateTime EndDate { get; set; }
                public string LicenseStatus { get; set; }
                public string LicensorType { get; set; }
            }

            public class Licenses
            {
                public List<License> License { get; set; }
            }

            public class Root
            {
                public GetStakeholderInfoResult GetStakeholderInfoResult { get; set; }
            }

            public class Stakeholder
            {
                public string Id { get; set; }
                public string NameAR { get; set; }
                public string NameEN { get; set; }
                public string IdentityNumber { get; set; }
                public string DateOfBirth { get; set; }
                public string PhoneNumber { get; set; }
                public string MobileNumber { get; set; }
                public string BanningStatus { get; set; }
                public string BanningReason { get; set; }
                public string Gender { get; set; }
                public string Nationality { get; set; }
                public string PassportNumber { get; set; }
                public Licenses Licenses { get; set; }
            }

            public class Stakeholders
            {
                public List<Stakeholder> Stakeholder { get; set; }
            }




        }

        public class AJMLiscenceDetails
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
            public class ActivityListInfo
            {
                public string Serial { get; set; }
                public string NameAR { get; set; }
                public string NameEN { get; set; }
                public string ORA_CODNO { get; set; }
            }

            public class GetTransactionInfoResponse
            {
                public GetTransactionInfoResult GetTransactionInfoResult { get; set; }
            }

            public class GetTransactionInfoResult
            {
                public NewDataSet NewDataSet { get; set; }
            }

            public class NewDataSet
            {
                public TransactionStatus TransactionStatus { get; set; }
                public List<ActivityListInfo> Activity_List_Info { get; set; }
                public List<OwnersListInfo> Owners_List_Info { get; set; }
                public List<TransactionInfo> Transaction_Info { get; set; }
            }

            public class OwnersListInfo
            {
                public string Serial { get; set; }
                public string NameAR { get; set; }
                public string NameEN { get; set; }
                public string Nationality_Serial { get; set; }
                public string Nationality_Ar { get; set; }
                public string Gender_Serial { get; set; }
                public string Gender_Ar { get; set; }
                public string IdentityNumber { get; set; }
                public string PassportNumber { get; set; }
                public string PhoneNumber { get; set; }
                public string MobileNumber { get; set; }
                public string Notes { get; set; }
                public string OwnerType_Serial { get; set; }
                public string OwnerType_Ar { get; set; }
                public string LeasorType_Serial { get; set; }
                public string LeasorType_Ar { get; set; }
                public string LicenseSource { get; set; }
            }

            public class Root
            {
                public GetTransactionInfoResponse GetTransactionInfoResponse { get; set; }
            }

            public class TransactionInfo
            {
                public string Transaction_Serial { get; set; }
                public string TransactionAr { get; set; }
            }

            public class TransactionStatus
            {
                public string LicenseNo { get; set; }
                public string ApplicationNo { get; set; }
                public string TradeNameAR { get; set; }
                public string TradeNameEN { get; set; }
                public string LicenseType_Serial { get; set; }
                public string LegalForm_Serial { get; set; }
                public string LicenseStatus_Serial { get; set; }
                public DateTime StartDate { get; set; }
                public DateTime EndDate { get; set; }
                public string MobileNumber { get; set; }
                public string POBoxNo { get; set; }
                public string TelNo { get; set; }
                public string EmailAddress { get; set; }
                public string Id { get; set; }
                public string BanningStatus { get; set; }
                public string BanningReason { get; set; }
                public string isTaziz { get; set; }
                public string LicenseClassification { get; set; }
                public string LicenseClassificationSerial { get; set; }
                public string ChamberNumber { get; set; }
                public string RegistrationNumber { get; set; }
            }


        }
    }
}