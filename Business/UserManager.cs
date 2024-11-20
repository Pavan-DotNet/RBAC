using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using MOCDIntegrations.Auth;

namespace MOCDIntegrations.Business
{
    public class UserManager
    {
        private readonly string _connectionString;

        public UserManager()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["INTEGRATION_CONN"].ConnectionString;
        }

        public List<string> GetUserPermissions(int userId)
        {
            // Existing GetUserPermissions method...
        }

        public List<string> GetAvailableRoles()
        {
            var roles = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT RoleName FROM Roles", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return roles;
        }

        // Other existing methods...
    }
}
