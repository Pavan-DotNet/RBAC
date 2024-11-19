
using System;
using System.Data.SqlClient;
using System.Configuration;

namespace MOCDIntegrations.Auth
{
    public class SqlServerAuthProvider
    {
        private string connectionString;

        public SqlServerAuthProvider()
        {
            connectionString = ConfigurationManager.ConnectionStrings["INTEGRATION_CONN"].ConnectionString;
        }

        public bool ValidateUser(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password", connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password); // Note: In production, use hashed passwords

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
    }
}
