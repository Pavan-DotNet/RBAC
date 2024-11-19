using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MOCDIntegrations.Models
{
    public class MOEEducationDetails
    {
        public class ChildProtectionUnitDatum
        {
            public string dateOfTheIncidentCreation { get; set; }
            public string incidentNumber { get; set; }
            public string childFullName { get; set; }
            public string studentID { get; set; }
            public string studentEmirateID { get; set; }
            public string typeAndDegreeOfAbuse { get; set; }
            public bool Success { get; set; }
        }
    }
}