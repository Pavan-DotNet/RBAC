using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class DMATDetails
    {

        public class DMATRequest
        {
            public string EmiratesId { get; set; }
        }
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class AcquisitionTypeHistory
        {
            public double? percentOwnership { get; set; }
            public string acquisitionDate { get; set; }
            public string acquiisitionChange { get; set; }
        }

        public class PlotOwnershipDetails
        {
            public int plotId { get; set; } = 0;
            public string municipalityId { get; set; } 
            public string municipalityNameA { get; set; } 
            public string municipalityNameE { get; set; } 
            public string districtId { get; set; } 
            public string districtNameA { get; set; } 
            public string districtNameE { get; set; } 
            public string districtNumber { get; set; } 
            public string communityId { get; set; } 
            public string communityNameA { get; set; } 
            public string communityNameE { get; set; } 
            public string communityNumber { get; set; } 
            public string roadId { get; set; } 
            public string roadNumber { get; set; } 
            public string roadNameA { get; set; } 
            public string roadNameE { get; set; } 
            public string plotNumber { get; set; } 
            public string plotAddress { get; set; } 
            public string landuseId { get; set; } 
            public string landuseNameA { get; set; } 
            public string landuseNameE { get; set; } 
            public string landuseConst { get; set; } 
            public string landuseMepsIdentifier { get; set; } 
            public string parentLanduseId { get; set; } 
            public string parentLanduseNameA { get; set; } 
            public string parentLanduseNameE { get; set; } 
            public string ownerId { get; set; } 
            public string plotTransactionId { get; set; } 
            public string plotOwnerShareId { get; set; } 
            public string rightsHoldTypeId { get; set; } 
            public string acquisitionTypeId { get; set; } 
            public string percentOwnership { get; set; } 
            public string priorityValue { get; set; } 
            public string ownershipRemarks { get; set; } 
            public string landFileNumber { get; set; } 
            public string rightsHoldTypeNameA { get; set; } 
            public string rightsHoldTypeNameE { get; set; } 
            public string acquisitionTypeA { get; set; } 
            public string acquisitionTypeE { get; set; } 
            public string userTitleId { get; set; } 
            public string userTitleNameA { get; set; } 
            public string userTitleNameE { get; set; } 
            public string userTitleDisplay { get; set; } 
            public string ownerNameA { get; set; } 
            public string ownerNameE { get; set; } 
            public string tribeNameA { get; set; } 
            public string tribeNameE { get; set; } 
            public string familyBookCity { get; set; } 
            public string familyBookIssueDate { get; set; } 
            public string familyBookNumber { get; set; } 
            public string familyPageNumber { get; set; } 
            public string tradeLicenseNumber { get; set; } 
            public string tradeLicenseExpireDate { get; set; } 
            public string chamberNumberId { get; set; } 
            public string nationalNumber { get; set; } 
            public string moiUnifiedNumber { get; set; } 
            public string nationalityNameA { get; set; } 
            public string nationalityNameE { get; set; } 
            public string gender { get; set; } 
            public string birthday { get; set; } 
            public string nameMother { get; set; } 
            public string townPlanningArchiveNumber { get; set; } 
            public string ownerType { get; set; } 
            public string acquisitionTypeConst { get; set; } 
            public string rightsholdtypeconst { get; set; } 
            public List<AcquisitionTypeHistory> acquisitionTypeHistory { get; set; }
        }

        public class Root
        {
            public PlotOwnershipDetails plotOwnershipDetails { get; set; }
            public int status { get; set; }
            public string responseMessage { get; set; }
            public int errorCode { get; set; }
        }
        public class ResponseAccessToken
        {
            public string username { get; set; }
            public string token { get; set; }

            public List<string> roles { get; set; }
            public string token_type { get; set; }
            public string access_token { get; set; }
            public int expires_in { get; set; }
            public string refresh_token { get; set; }
        }
    }
}