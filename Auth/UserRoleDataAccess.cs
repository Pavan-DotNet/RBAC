using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace MOCDIntegrations.Auth
{
    public class UserRoleDataAccess
    {
        private readonly string connectionString;

        public UserRoleDataAccess()
        {
            connectionString = ConfigurationManager.ConnectionStrings["INTEGRATION_CONN"].ConnectionString;
        }

        public List<UserRole> GetUserRoles(int userId)
        {
            List<UserRole> userRoles = new List<UserRole>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"SELECT ur.UserRoleId, ur.UserId, ur.RoleId, r.RoleName 
                                 FROM UserRoles ur
                                 JOIN Roles r ON ur.RoleId = r.RoleId
                                 WHERE ur.UserId = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            userRoles.Add(new UserRole
                            {
                                UserRoleId = reader.GetInt32(0),
                                UserId = reader.GetInt32(1),
                                RoleId = reader.GetInt32(2),
                                RoleName = reader.GetString(3)
                            });
                        }
                    }
                }
            }

            return userRoles;
        }

        // Other existing methods...
    }

    public class UserRole
    {
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
