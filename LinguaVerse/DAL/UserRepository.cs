using Dapper;
using System.Collections.Generic;
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
            using var transaction = connection.BeginTransaction();

            try
            {
                var userId = await connection.ExecuteScalarAsync<int>(
                    "INSERT INTO \"User\" (username, password) VALUES (@Username, @Password) RETURNING userID",
                    new { Username = username, Password = password },
                    transaction
                );

                // Initialize DailyStreaks for the new user
                await connection.ExecuteAsync(
                    "INSERT INTO \"DailyStreaks\" (userID, day) VALUES (@UserID, 'Mon'), (@UserID, 'Tue'), (@UserID, 'Wed'), (@UserID, 'Thu'), (@UserID, 'Fri'), (@UserID, 'Sat'), (@UserID, 'Sun')",
                    new { UserID = userId },
                    transaction
                );

                // Initialize CourseProgress for the new user
                await connection.ExecuteAsync(
                    "INSERT INTO \"CourseProgress\" (userID, courseName, progress, level) VALUES (@UserID, 'English', 0.0, 'Beginner'), (@UserID, 'Italian', 0.0, 'Beginner')",
                    new { UserID = userId },
                    transaction
                );

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
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

        public async Task<User> GetUserByUsername(string username)
        {
            using var connection = _database.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<User>(
                "SELECT * FROM \"User\" WHERE username = @Username",
                new { Username = username }
            );
        }

        public async Task<IEnumerable<string>> GetDailyStreaks(int userId)
        {
            using var connection = _database.CreateConnection();
            return await connection.QueryAsync<string>(
                "SELECT day FROM \"DailyStreaks\" WHERE userID = @UserId",
                new { UserId = userId }
            );
        }

        public async Task<IEnumerable<CourseProgress>> GetCourseProgress(int userId)
        {
            using var connection = _database.CreateConnection();
            return await connection.QueryAsync<CourseProgress>(
                "SELECT courseName, progress, level FROM \"CourseProgress\" WHERE userID = @UserId",
                new { UserId = userId }
            );
        }

        public async Task<IEnumerable<FeaturedCourse>> GetFeaturedCourses()
        {
            using var connection = _database.CreateConnection();
            return await connection.QueryAsync<FeaturedCourse>(
                "SELECT courseName, duration, questions, level, \"flagIcon\" FROM \"FeaturedCourses\""
            );
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

        public class CourseProgress
        {
            public string CourseName { get; set; }
            public float Progress { get; set; }
            public string Level { get; set; }
        }

        public class FeaturedCourse
        {
            public string CourseName { get; set; }
            public int Duration { get; set; }
            public int Questions { get; set; }
            public string Level { get; set; }
            public string FlagIcon { get; set; }
        }
    }
}
