
using System.Collections.Generic;

namespace MOCDIntegrations.Models.FEWA
{
    public class Root
    {
        public List<AccountDetail> AccountDetails { get; set; }
    }
    public class Master
    {
        public AccountDetail AccountDetails { get; set; }
    }
}
