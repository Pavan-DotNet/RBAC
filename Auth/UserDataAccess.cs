
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MOCDIntegrations.Models;

namespace MOCDIntegrations.Auth
{
    public class UserDataAccess
    {
        private readonly string _connectionString;

        public UserDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User GetUserById(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT u.UserId, u.Username, u.Email, r.RoleId, r.RoleName
                    FROM Users u
                    LEFT JOIN UserRoles ur ON u.UserId = ur.UserId
                    LEFT JOIN Roles r ON ur.RoleId = r.RoleId
                    WHERE u.UserId = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                User user = null;
                while (reader.Read())
                {
                    if (user == null)
                    {
                        user = new User
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            Roles = new List<Role>()
                        };
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("RoleId")))
                    {
                        user.Roles.Add(new Role
                        {
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"].ToString()
                        });
                    }
                }

                return user;
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT u.UserId, u.Username, u.Email, r.RoleId, r.RoleName
                    FROM Users u
                    LEFT JOIN UserRoles ur ON u.UserId = ur.UserId
                    LEFT JOIN Roles r ON ur.RoleId = r.RoleId";
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                User currentUser = null;
                while (reader.Read())
                {
                    int userId = Convert.ToInt32(reader["UserId"]);
                    if (currentUser == null || currentUser.UserId != userId)
                    {
                        currentUser = new User
                        {
                            UserId = userId,
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            Roles = new List<Role>()
                        };
                        users.Add(currentUser);
                    }

                    if (!reader.IsDBNull(reader.GetOrdinal("RoleId")))
                    {
                        currentUser.Roles.Add(new Role
                        {
                            RoleId = Convert.ToInt32(reader["RoleId"]),
                            RoleName = reader["RoleName"].ToString()
                        });
                    }
                }
            }

            return users;
        }

        public int CreateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password); SELECT SCOPE_IDENTITY()";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password); // Note: In production, use hashed passwords

                connection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public void UpdateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Users SET Username = @Username, Email = @Email WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", user.UserId);
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Email", user.Email);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "DELETE FROM Users WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
