using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using MOCDIntegrations.Auth;

namespace MOCDIntegrations.Business
{
    public class RoleManager
    {
        private readonly string _connectionString;

        public RoleManager()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["INTEGRATION_CONN"].ConnectionString;
        }

        public Role GetRoleByName(string roleName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT RoleId, RoleName, Description FROM Roles WHERE RoleName = @RoleName", connection))
                {
                    command.Parameters.AddWithValue("@RoleName", roleName);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Role
                            {
                                RoleId = reader.GetInt32(0),
                                RoleName = reader.GetString(1),
                                Description = reader.GetString(2)
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Other existing methods...
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}
