using System.Collections.Generic;

namespace MOCDIntegrations.Models
{
    public class UserRolesViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public List<string> AssignedRoles { get; set; }
        public List<string> AvailableRoles { get; set; }
    }
}
