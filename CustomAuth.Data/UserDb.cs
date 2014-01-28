using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace CustomAuth.Data
{
    public class UserDb
    {
        private readonly string _connectionString;

        public UserDb(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void AddUser(User user)
        {
            var sql = "INSERT INTO Users (FirstName, LastName, Email, Username, Password,Salt) " +
                      " VALUES (@firstName, @lastName, @email, @username, @password, @salt); SELECT @@Identity";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                //IEnumerable<User> users = connection.Query<User>("SELECT * FROM Users");
                //connection.Execute(sql, user);
                int newId = (int)(connection.Query<decimal>(sql, user).First());
                user.Id = newId;
            }
        }

        public User GetUser(int id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @userId";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<User>(sql, new { userId = id }).FirstOrDefault();
            }
        }

        public User GetUser(string username)
        {
            var sql = "SELECT * FROM Users WHERE Username= @username";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<User>(sql, new {username = username}).FirstOrDefault();
            }
        }
    }
}