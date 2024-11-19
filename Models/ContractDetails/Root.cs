
namespace MOCDIntegrations.Models.ContractDetails
{
    public class Root
    {
        public OutParameters OutParameters { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class RootError
    {
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
        public string description { get; set; }
    }


}
