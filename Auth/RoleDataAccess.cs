
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MOCDIntegrations.Models;

namespace MOCDIntegrations.Auth
{
    public class RoleDataAccess
    {
        private readonly string _connectionString;

        public RoleDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Role> GetAllRoles()
        {
            List<Role> roles = new List<Role>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT RoleId, RoleName, Description FROM Roles";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(new Role
                    {
                        RoleId = Convert.ToInt32(reader["RoleId"]),
                        RoleName = reader["RoleName"].ToString(),
                        Description = reader["Description"].ToString()
                    });
                }
            }

            return roles;
        }

        public Role GetRoleById(int roleId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT RoleId, RoleName, Description FROM Roles WHERE RoleId = @RoleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoleId", roleId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Role
                    {
                        RoleId = Convert.ToInt32(reader["RoleId"]),
                        RoleName = reader["RoleName"].ToString(),
                        Description = reader["Description"].ToString()
                    };
                }
            }

            return null;
        }

        public int CreateRole(Role role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Roles (RoleName, Description) VALUES (@RoleName, @Description); SELECT SCOPE_IDENTITY()";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoleName", role.RoleName);
                command.Parameters.AddWithValue("@Description", role.Description);

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void UpdateRole(Role role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Roles SET RoleName = @RoleName, Description = @Description WHERE RoleId = @RoleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoleId", role.RoleId);
                command.Parameters.AddWithValue("@RoleName", role.RoleName);
                command.Parameters.AddWithValue("@Description", role.Description);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteRole(int roleId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Roles WHERE RoleId = @RoleId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@RoleId", roleId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
