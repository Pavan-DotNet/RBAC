
namespace MOCDIntegrations.Models.VehicleCount
{
    public class Root
    {
        public OutParameters OutParameters { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Description
    {
        public Exception Exception { get; set; }
    }

    public class Exception
    {
        public string ExceptionCode { get; set; }
        public string ExceptionDescription { get; set; }
    }

    public class RootError
    {
        public string errorCode { get; set; }
        public string errorMsg { get; set; }
        public Description description { get; set; }
    }


}
