using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class ADHADetails
    {
        public class ADHADetailsRequestParams
        {
            public string EmiratesID { get; set; }

        }
        public class ADHAResponse
        {

            public class RequestedService
            {

                public string Name { get; set; }

                public string Text { get; set; }
                public string RequestedService_Text { get; set; }


            }

            public class ApprovedService
            {

                public string Name { get; set; }

                public string ApprovedService_Text { get; set; }
            }

            public class ApplicationStage
            {

                public string Name { get; set; }

                public string ApplicationStage_Text { get; set; }
            }

            public class ApplicationStatus
            {

                public string Name { get; set; }

                public string Text { get; set; }
            }

            public class ApplicantInfo
            {

                public string EmiratesID { get; set; }

                public string ApplicantType { get; set; }

                public string ApplicantNameEn { get; set; }

                public string ApplicantNameAr { get; set; }

                public string PendingActions { get; set; }
            }

            //public class ApplicantsInfo
            //{

            //    public ApplicantInfo ApplicantInfo { get; set; }
            //}

            public class Item
            {

                public string CbaNumber { get; set; }

                public string ReferenceId { get; set; }

                public string SystemCode { get; set; }

                public string DateOfCreation { get; set; }

                public RequestedService RequestedService { get; set; }

                public ApprovedService ApprovedService { get; set; }

                public string PrimaryApplicantFullNameArabic { get; set; }

                public string PrimaryApplicantFullNameEnglish { get; set; }

                public ApplicationStage ApplicationStage { get; set; }

                public ApplicationStatus ApplicationStatus { get; set; }

                public string ResponseCode { get; set; }

                public string ResponseMessage { get; set; }

                public string FGBDetails { get; set; }

                public string item_Id { get; set; }
                public string SearchResponseByEmirate_Id { get; set; }

                public string LastUpdateDate { get; set; }

                public string ServiceLocation { get; set; }

                public string ApplicationStageStatus { get; set; }

                public string ApplicantType { get; set; }

                public ApplicantInfo ApplicantsInfo { get; set; }

                public string AppStageStatusTextEnglish { get; set; }

                public string AppStageStatusTextArabic { get; set; }

                public bool HasActiveInviteOnApp { get; set; }

                public bool HasActiveWifeConsentOnApp { get; set; }
            }

            public class SearchResponseByEmirate
            {

                public Item Item { get; set; }

                public string Ns0 { get; set; }
                public string Text { get; set; }
            }

            public class Body
            {

                public SearchResponseByEmirate SearchResponseByEmirate { get; set; }
            }

            public class Envelope
            {

                public Body Body { get; set; }

                public string Soap { get; set; }

                public string Text { get; set; }
            }


        }
    }
}