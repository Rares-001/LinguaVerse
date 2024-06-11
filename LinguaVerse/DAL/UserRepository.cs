using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using LinguaVerse.Model;
using System.Collections.ObjectModel;

namespace LinguaVerse.DAL
{
    public class UserRepository
    {
        private readonly IDbConnection _connection;

        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            using var transaction = _connection.BeginTransaction();
            try
            {
                var userId = await _connection.ExecuteScalarAsync<int>(
                    "INSERT INTO \"User\" (username, password) VALUES (@Username, @Password) RETURNING userID",
                    new { Username = username, Password = password },
                    transaction
                );

                // Initialize DailyStreaks for the new user
                await _connection.ExecuteAsync(
                    "INSERT INTO \"DailyStreaks\" (userID, day) VALUES (@UserID, 'Mon'), (@UserID, 'Tue'), (@UserID, 'Wed'), (@UserID, 'Thu'), (@UserID, 'Fri'), (@UserID, 'Sat'), (@UserID, 'Sun')",
                    new { UserID = userId },
                    transaction
                );

                // Initialize CourseProgress for the new user
                await _connection.ExecuteAsync(
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
            var user = await _connection.QuerySingleOrDefaultAsync<User>(
                "SELECT * FROM \"User\" WHERE username = @Username AND password = @Password",
                new { Username = username, Password = password }
            );

            return user != null;
        }

        public async Task<int> GetUserIdByUsername(string username)
        {
            return await _connection.ExecuteScalarAsync<int>(
                "SELECT userID FROM \"User\" WHERE username = @Username",
                new { Username = username }
            );
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _connection.QuerySingleOrDefaultAsync<User>(
                "SELECT * FROM \"User\" WHERE username = @Username",
                new { Username = username }
            );
        }

        public async Task<User> GetUserById(int userId)
        {
            const string query = "SELECT * FROM \"User\" WHERE userID = @UserId";
            var user = await _connection.QuerySingleOrDefaultAsync<User>(query, new { UserId = userId });
            if (user == null)
            {
                System.Diagnostics.Debug.WriteLine($"User with ID {userId} not found in the database.");
            }
            return user;
        }


        public async Task<IEnumerable<string>> GetDailyStreaks(int userId)
        {
            return await _connection.QueryAsync<string>(
                "SELECT day FROM \"DailyStreaks\" WHERE userID = @UserId",
                new { UserId = userId }
            );
        }

        public async Task UpdateDailyStreak(int userId, string day, bool completed)
        {
            const string query = "UPDATE \"DailyStreaks\" SET completed = @Completed WHERE userID = @UserId AND day = @Day";
            await _connection.ExecuteAsync(query, new { UserId = userId, Day = day, Completed = completed });
        }


        public async Task<IEnumerable<CourseProgress>> GetCourseProgress(int userId)
        {
            return await _connection.QueryAsync<CourseProgress>(
                "SELECT courseName, progress, level FROM \"CourseProgress\" WHERE userID = @UserId",
                new { UserId = userId }
            );
        }

        public async Task<IEnumerable<FeaturedCourse>> GetFeaturedCourses()
        {
            return await _connection.QueryAsync<FeaturedCourse>(
                "SELECT courseName, duration, questions, level, \"flagIcon\" FROM \"FeaturedCourses\""
            );
        }

        public async Task<IEnumerable<UserProgress>> GetUserProgressAsync(int userId)
        {
            const string query = "SELECT * FROM \"public\".\"UserProgress\" WHERE \"UserID\" = @UserId";
            return await _connection.QueryAsync<UserProgress>(query, new { UserId = userId });
        }


        public async Task SaveUserProgressAsync(UserProgress userProgress)
        {
            const string query = "INSERT INTO \"UserProgress\" (\"UserID\", \"QuizID\", \"Score\", \"CompletionTime\", \"AttemptDate\") VALUES (@UserID, @QuizID, @Score, @CompletionTime, @AttemptDate)";
            await _connection.ExecuteAsync(query, userProgress);
        }

        public async Task<bool> ResetPassword(string username, string newPassword)
        {
            var result = await _connection.ExecuteAsync(
                "UPDATE \"User\" SET password = @NewPassword WHERE username = @Username",
                new { Username = username, NewPassword = newPassword }
            );

            return result > 0;
        }

        public async Task<bool> UpdateProfile(int userId, string username, string password, string languagePreference, float progress)
        {
            var result = await _connection.ExecuteAsync(
                "UPDATE \"User\" SET username = @Username, password = @Password, languagePreference = @LanguagePreference, progress = @Progress WHERE userID = @UserId",
                new { UserId = userId, Username = username, Password = password, LanguagePreference = languagePreference, Progress = progress }
            );

            return result > 0;
        }

        public async Task<bool> TestDatabaseConnection()
        {
            try
            {
                if (_connection is NpgsqlConnection npgsqlConnection)
                {
                    await npgsqlConnection.OpenAsync(CancellationToken.None);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesAsync()
        {
            const string query = "SELECT * FROM \"Quizzes\"";
            return await _connection.QueryAsync<Quiz>(query);
        }

        public async Task<IEnumerable<Question>> GetQuestionsAsync(int quizId)
        {
            const string query = "SELECT * FROM \"Questions\" WHERE \"QuizID\" = @QuizID";
            var questions = await _connection.QueryAsync<Question>(query, new { QuizID = quizId });

            // Convert Choices from string array to ObservableCollection<string>
            foreach (var question in questions)
            {
                question.Choices = new ObservableCollection<string>(question.Choices); 
            }

            return questions;
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
