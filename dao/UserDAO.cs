using TrafficViolationApp.model;
using Microsoft.Data.SqlClient;
using BCrypt.Net;

namespace TrafficViolationApp.dao
{
    class UserDAO : IDAOInterface<User, int>
    {
        private readonly DbContext dbContext = new DbContext();
        public int insert(User user)
        {
            string query = "INSERT INTO Users (FullName, Email, Password, Role, Phone, Address) " +
                           "VALUES (@FullName, @Email, @Password, @Role, @Phone, @Address)";

            try
            {
                using (var connection = dbContext.GetConnection())
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FullName", user.FullName);
                    command.Parameters.AddWithValue("@Email", user.Email);

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password, 10);
                    command.Parameters.AddWithValue("@Password", hashedPassword);

                    command.Parameters.AddWithValue("@Role", user.Role);
                    command.Parameters.AddWithValue("@Phone", user.Phone);
                    command.Parameters.AddWithValue("@Address", user.Address);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return -1;
            }
        }

        public int delete(User user)
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";

            using (var connection = dbContext.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", user.UserID);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int update(User user)
        {
            string query = "UPDATE Users SET FullName = @FullName, Email = @Email, Password = @Password, " +
                           "Role = @Role, Phone = @Phone, Address = @Address WHERE UserID = @UserID";

            using (var connection = dbContext.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", user.UserID);
                command.Parameters.AddWithValue("@FullName", user.FullName);
                command.Parameters.AddWithValue("@Email", user.Email);
                command.Parameters.AddWithValue("@Password", user.Password);
                command.Parameters.AddWithValue("@Role", user.Role);
                command.Parameters.AddWithValue("@Phone", user.Phone);
                command.Parameters.AddWithValue("@Address", user.Address);

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public List<User> selectAll()
        {
            List<User> users = new List<User>();
            string query = "SELECT * FROM Users";

            using (var connection = dbContext.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            FullName = reader["FullName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = reader["Role"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString()
                        });
                    }
                }
            }
            return users;
        }

       

        public bool checkLogin(string email, string password)
        {
            string query = "SELECT Password FROM Users WHERE Email = @Email";

            using (var connection = dbContext.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", email);
                connection.Open();

                var storedPasswordHash = command.ExecuteScalar() as string;
                if (storedPasswordHash == null)
                {
                    return false;
                }
                return BCrypt.Net.BCrypt.Verify(password, storedPasswordHash);
            }
        }

        public User selectById(int id)
        {
            string query = "SELECT * FROM Users WHERE UserID = @UserID";

            using (var connection = dbContext.GetConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", id);
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            FullName = reader["FullName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Password = reader["Password"].ToString(),
                            Role = reader["Role"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            Address = reader["Address"].ToString()
                        };
                    }
                }
            }
            return null;
        }
    }
}
