// DAL/UserRepository.cs
using Dapper;
using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace LinguaVerse.DAL
{
    public class UserRepository
    {
        private readonly Database _database;

        public UserRepository(Database database)
        {
            _database = database;
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            using var connection = _database.CreateConnection();
            var result = await connection.ExecuteAsync(
                "INSERT INTO \"User\" (username, password) VALUES (@Username, @Password)",
                new { Username = username, Password = password }
            );

            return result > 0;
        }

        public async Task<bool> LoginUser(string username, string password)
        {
            using var connection = _database.CreateConnection();
            var user = await connection.QuerySingleOrDefaultAsync<User>(
                "SELECT * FROM \"User\" WHERE username = @Username AND password = @Password",
                new { Username = username, Password = password }
            );

            return user != null;
        }

        public async Task<bool> ResetPassword(string username, string newPassword)
        {
            using var connection = _database.CreateConnection();
            var result = await connection.ExecuteAsync(
                "UPDATE \"User\" SET password = @NewPassword WHERE username = @Username",
                new { Username = username, NewPassword = newPassword }
            );

            return result > 0;
        }

        public async Task<bool> UpdateProfile(int userId, string username, string password, string languagePreference, float progress)
        {
            using var connection = _database.CreateConnection();
            var result = await connection.ExecuteAsync(
                "UPDATE \"User\" SET username = @Username, password = @Password, languagePreference = @LanguagePreference, progress = @Progress WHERE userID = @UserId",
                new { UserId = userId, Username = username, Password = password, LanguagePreference = languagePreference, Progress = progress }
            );

            return result > 0;
        }

        public async Task<bool> TestDatabaseConnection()
        {
            try
            {
                using var connection = _database.CreateConnection() as NpgsqlConnection;
                if (connection != null)
                {
                    await connection.OpenAsync(CancellationToken.None); // Pass CancellationToken.None for testing purposes 
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public class User
        {
            public int UserID { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string LanguagePreference { get; set; }
            public float Progress { get; set; }
        }
    }
}
