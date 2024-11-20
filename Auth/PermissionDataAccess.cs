
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MOCDIntegrations.Models;

namespace MOCDIntegrations.Auth
{
    public class PermissionDataAccess
    {
        private readonly string _connectionString;

        public PermissionDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Permission> GetAllPermissions()
        {
            List<Permission> permissions = new List<Permission>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT PermissionId, PermissionName, Description FROM Permissions";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    permissions.Add(new Permission
                    {
                        PermissionId = Convert.ToInt32(reader["PermissionId"]),
                        PermissionName = reader["PermissionName"].ToString(),
                        Description = reader["Description"].ToString()
                    });
                }
            }

            return permissions;
        }

        public Permission GetPermissionById(int permissionId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT PermissionId, PermissionName, Description FROM Permissions WHERE PermissionId = @PermissionId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PermissionId", permissionId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Permission
                    {
                        PermissionId = Convert.ToInt32(reader["PermissionId"]),
                        PermissionName = reader["PermissionName"].ToString(),
                        Description = reader["Description"].ToString()
                    };
                }
            }

            return null;
        }

        public int CreatePermission(Permission permission)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Permissions (PermissionName, Description) VALUES (@PermissionName, @Description); SELECT SCOPE_IDENTITY()";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PermissionName", permission.PermissionName);
                command.Parameters.AddWithValue("@Description", permission.Description);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void UpdatePermission(Permission permission)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Permissions SET PermissionName = @PermissionName, Description = @Description WHERE PermissionId = @PermissionId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PermissionId", permission.PermissionId);
                command.Parameters.AddWithValue("@PermissionName", permission.PermissionName);
                command.Parameters.AddWithValue("@Description", permission.Description);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeletePermission(int permissionId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Permissions WHERE PermissionId = @PermissionId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PermissionId", permissionId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
