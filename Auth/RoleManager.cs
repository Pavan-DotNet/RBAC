

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace MOCDIntegrations.Auth
{
    public class RoleManager
    {
        private string connectionString;

        public RoleManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["INTEGRATION_CONN"].ConnectionString;
        }

        public List<string> GetAllRoles()
        {
            List<string> roles = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT RoleName FROM Roles", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
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

        public void CreateRole(string roleName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO Roles (RoleName) VALUES (@RoleName)", connection))
                {
                    command.Parameters.AddWithValue("@RoleName", roleName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRole(string roleName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("DELETE FROM Roles WHERE RoleName = @RoleName", connection))
                {
                    command.Parameters.AddWithValue("@RoleName", roleName);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetUserRoles(int userId)
        {
            List<string> roles = new List<string>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"
                    SELECT r.RoleName 
                    FROM UserRoles ur
                    JOIN Roles r ON ur.RoleId = r.RoleId
                    WHERE ur.UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
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

        public void UpdateUserRoles(int userId, List<string> selectedRoles)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Remove all existing roles for the user
                        using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM UserRoles WHERE UserId = @UserId", connection, transaction))
                        {
                            deleteCommand.Parameters.AddWithValue("@UserId", userId);
                            deleteCommand.ExecuteNonQuery();
                        }

                        // Add new roles for the user
                        foreach (string role in selectedRoles)
                        {
                            using (SqlCommand insertCommand = new SqlCommand(@"
                                INSERT INTO UserRoles (UserId, RoleId)
                                SELECT @UserId, RoleId FROM Roles WHERE RoleName = @RoleName", connection, transaction))
                            {
                                insertCommand.Parameters.AddWithValue("@UserId", userId);
                                insertCommand.Parameters.AddWithValue("@RoleName", role);
                                insertCommand.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}

