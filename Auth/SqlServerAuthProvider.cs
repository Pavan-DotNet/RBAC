


using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using MOCDIntegrations.Models;

namespace MOCDIntegrations.Auth
{
    public class SqlServerAuthProvider
    {
        private string connectionString;

        public SqlServerAuthProvider()
        {
            connectionString = ConfigurationManager.ConnectionStrings["INTEGRATION_CONN"].ConnectionString;
        }

        public (bool isValid, List<string> roles) ValidateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"
                    SELECT r.RoleName 
                    FROM Users u
                    LEFT JOIN UserRoles ur ON u.UserId = ur.UserId
                    LEFT JOIN Roles r ON ur.RoleId = r.RoleId
                    WHERE u.Username = @Username AND u.Password = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password); // Note: In production, use hashed passwords

                    List<string> roles = new List<string>();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                roles.Add(reader.GetString(0));
                            }
                        }
                    }

                    return (roles.Count > 0, roles);
                }
            }
        }

        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT UserId, Username FROM Users", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1)
                            });
                        }
                    }
                }
            }
            return users;
        }

        public User GetUserById(int userId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT UserId, Username FROM Users WHERE UserId = @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1)
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}


