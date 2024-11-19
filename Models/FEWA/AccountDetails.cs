
namespace MOCDIntegrations.Models.FEWA
{
    public class AccountDetail
    {
        public object EmiratesID { get; set; }
        public int BusinessPartnerNo { get; set; }
        public object ContractAccountNo { get; set; }
        public string Category { get; set; }
        public string AccountClass { get; set; }
        public object SpecialCode { get; set; }
        public string SpecialNeed { get; set; }
        public object PremiseNo { get; set; }
        public object MoveInDate { get; set; }
        public object MoveOutDate { get; set; }
        public object PE_StartingDate { get; set; }
        public string Email { get; set; }
        public string MobileNo { get; set; }
        public string Latitude_E { get; set; }
        public string Longitude_E { get; set; }
        public string Latitude_W { get; set; }
        public string Longitude_W { get; set; }
        public AddressDetails AddressDetails { get; set; }
        public LatestInvoice LatestInvoice { get; set; }
    }
}
