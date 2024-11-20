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
            var permissions = new List<string>();

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand(@"
                    SELECT DISTINCT p.PermissionName
                    FROM UserRoles ur
                    JOIN RolePermissions rp ON ur.RoleId = rp.RoleId
                    JOIN Permissions p ON rp.PermissionId = p.PermissionId
                    WHERE ur.UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            permissions.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return permissions;
        }

        // Other existing methods...
    }
}
