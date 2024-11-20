
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MOCDIntegrations.Models;

namespace MOCDIntegrations.Auth
{
    public class UserRoleDataAccess
    {
        private readonly string _connectionString;

        public UserRoleDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<UserRole> GetUserRoles(int userId)
        {
            List<UserRole> userRoles = new List<UserRole>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT ur.UserRoleId, ur.UserId, ur.RoleId, r.RoleName
                    FROM UserRoles ur
                    INNER JOIN Roles r ON ur.RoleId = r.RoleId
                    WHERE ur.UserId = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    userRoles.Add(new UserRole
                    {
                        UserRoleId = Convert.ToInt32(reader["UserRoleId"]),
                        UserId = Convert.ToInt32(reader["UserId"]),
                        RoleId = Convert.ToInt32(reader["RoleId"]),
                        RoleName = reader["RoleName"].ToString()
                    });
                }
            }

            return userRoles;
        }

        public void AssignRoleToUser(int userId, int roleId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@RoleId", roleId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void RemoveRoleFromUser(int userId, int roleId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@RoleId", roleId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public bool UserHasRole(int userId, int roleId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@RoleId", roleId);

                connection.Open();
                int count = Convert.ToInt32(command.ExecuteScalar());

                return count > 0;
            }
        }
    }
}
