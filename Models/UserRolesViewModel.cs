
using System.Collections.Generic;

namespace MOCDIntegrations.Models
{
    public class UserRolesViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public List<string> AllRoles { get; set; }
        public List<string> UserRoles { get; set; }
    }
}
