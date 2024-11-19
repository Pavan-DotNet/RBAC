using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOCDIntegrations.Models.ChildViolence
{
    public class ChildViolenceCase
    {
        public string CaseId { get; set; }
        public string DateReceived { get; set; }
        public string DateActioned { get; set; }
        public string Source { get; set; }
        public string EstablishmentName { get; set; }
        public string Region { get; set; }
        public string ChildName { get; set; }
        public string ChildGrade { get; set; }
        public string ReportedCaseCategory { get; set; }
        public object ActualCaseCategory { get; set; }
        public string CaseSummary { get; set; }
        public string CaseStatus { get; set; }
        public string ClosureComments { get; set; }
    }
}
