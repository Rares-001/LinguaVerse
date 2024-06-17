using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using LinguaVerse.Model;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Collections.ObjectModel;

namespace LinguaVerse.DAL
{
    public class UserRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(string connectionString, ILogger<UserRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        private async Task<T> ExecuteWithConnectionAsync<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using var connection = CreateConnection();
                _logger.LogDebug("Opening database connection.");
                connection.Open();
                var result = await getData(connection);
                _logger.LogDebug("Closing database connection.");
                connection.Close();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database operation failed.");
                throw;
            }
        }

        private async Task ExecuteWithConnectionAsync(Func<IDbConnection, Task> execute)
        {
            try
            {
                using var connection = CreateConnection();
                _logger.LogDebug("Opening database connection.");
                connection.Open();
                await execute(connection);
                _logger.LogDebug("Closing database connection.");
                connection.Close();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database operation failed.");
                throw;
            }
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            _logger.LogInformation("Registering user with username: {Username}", username);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                using var transaction = connection.BeginTransaction();
                try
                {
                    var userId = await connection.ExecuteScalarAsync<int>(
                        "INSERT INTO \"User\" (username, password) VALUES (@Username, @Password) RETURNING userID",
                        new { Username = username, Password = password },
                        transaction
                    );

                    // Initialize daily streaks for the new user
                    await connection.ExecuteAsync(
                        "INSERT INTO \"DailyStreaks\" (userID, day, completed) VALUES (@UserID, 'Mon', false), (@UserID, 'Tue', false), (@UserID, 'Wed', false), (@UserID, 'Thu', false), (@UserID, 'Fri', false), (@UserID, 'Sat', false), (@UserID, 'Sun', false)",
                        new { UserID = userId },
                        transaction
                    );

                    await connection.ExecuteAsync(
                        "INSERT INTO \"CourseProgress\" (userID, courseName, progress, level) VALUES (@UserID, 'English', 0.0, 'Beginner'), (@UserID, 'Italian', 0.0, 'Beginner')",
                        new { UserID = userId },
                        transaction
                    );

                    transaction.Commit();
                    _logger.LogInformation("User registered successfully with ID: {UserID}", userId);
                    return true;
                }
                catch (Exception)
                {
                    _logger.LogWarning("Transaction failed. Rolling back.");
                    transaction.Rollback();
                    throw;
                }
            });
        }


        public async Task<bool> LoginUser(string username, string password)
        {
            _logger.LogInformation("Logging in user with username: {Username}", username);
            var user = await ExecuteWithConnectionAsync(async connection =>
            {
                return await connection.QuerySingleOrDefaultAsync<User>(
                    "SELECT * FROM \"User\" WHERE username = @Username AND password = @Password",
                    new { Username = username, Password = password }
                );
            });

            if (user == null)
            {
                _logger.LogWarning("Login failed for user: {Username}", username);
                return false;
            }

            _logger.LogInformation("Login successful for user: {Username}", username);
            return true;
        }

        public async Task<int> GetUserIdByUsername(string username)
        {
            _logger.LogInformation("Fetching user ID for username: {Username}", username);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                return await connection.ExecuteScalarAsync<int>(
                    "SELECT userID FROM \"User\" WHERE username = @Username",
                    new { Username = username }
                );
            });
        }

        public async Task<User> GetUserByUsername(string username)
        {
            _logger.LogInformation("Fetching user details for username: {Username}", username);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                var user = await connection.QuerySingleOrDefaultAsync<User>(
                    "SELECT * FROM \"User\" WHERE username = @Username",
                    new { Username = username }
                );

                if (user == null)
                {
                    _logger.LogWarning("User not found: {Username}", username);
                }
                else
                {
                    _logger.LogInformation("User found: {Username}, ID: {UserID}", username, user.UserID);
                }

                return user;
            });
        }

        public async Task<User> GetUserById(int userId)
        {
            _logger.LogInformation("Fetching user details for user ID: {UserID}", userId);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = "SELECT * FROM \"User\" WHERE userID = @UserId";
                var user = await connection.QuerySingleOrDefaultAsync<User>(query, new { UserId = userId });
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserID} not found.", userId);
                }
                return user;
            });
        }

        public async Task<IEnumerable<DailyStreak>> GetDailyStreaks(int userId)
        {
            using var connection = CreateConnection();
            const string query = "SELECT streakid AS StreakId, userid AS UserId, day, completed AS IsCompleted FROM \"DailyStreaks\" WHERE userid = @UserId";
            return await connection.QueryAsync<DailyStreak>(query, new { UserId = userId });
        }

        public async Task UpdateDailyStreak(int userId, string day, bool isCompleted)
        {
            _logger.LogInformation("Updating daily streak for user ID: {UserID}, day: {Day}, isCompleted: {IsCompleted}", userId, day, isCompleted);
            await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = "UPDATE \"DailyStreaks\" SET completed = @IsCompleted WHERE userid = @UserId AND day = @Day";
                _logger.LogDebug("Executing SQL: {Query} with parameters UserId: {UserId}, Day: {Day}, IsCompleted: {IsCompleted}", query, userId, day, isCompleted);
                var affectedRows = await connection.ExecuteAsync(query, new { UserId = userId, Day = day, IsCompleted = isCompleted });
                if (affectedRows > 0)
                {
                    _logger.LogInformation("Daily streak updated successfully for user ID: {UserID}, day: {Day}", userId, day);
                }
                else
                {
                    _logger.LogWarning("Failed to update daily streak for user ID: {UserID}, day: {Day}", userId, day);
                }
            });
        }


        public async Task<IEnumerable<CourseProgress>> GetCourseProgress(int userId)
        {
            _logger.LogInformation("Fetching course progress for user ID: {UserID}", userId);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                return await connection.QueryAsync<CourseProgress>(
                    "SELECT courseName, progress, level FROM \"CourseProgress\" WHERE userID = @UserId",
                    new { UserId = userId }
                );
            });
        }

        public async Task<IEnumerable<FeaturedCourse>> GetFeaturedCourses()
        {
            _logger.LogInformation("Fetching featured courses.");
            return await ExecuteWithConnectionAsync(async connection =>
            {
                return await connection.QueryAsync<FeaturedCourse>(
                    "SELECT courseName, duration, questions, level, \"flagIcon\" FROM \"FeaturedCourses\""
                );
            });
        }

        public async Task<IEnumerable<UserProgress>> GetUserProgressAsync(int userId)
        {
            _logger.LogInformation("Fetching user progress for user ID: {UserID}", userId);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = "SELECT * FROM \"UserProgress\" WHERE \"UserID\" = @UserId";
                return await connection.QueryAsync<UserProgress>(query, new { UserId = userId });
            });
        }




        public async Task SaveUserProgressAsync(UserProgress userProgress)
        {
            _logger.LogInformation("Saving user progress for user ID: {UserID}, Quiz ID: {QuizID}, Score: {Score}", userProgress.UserID, userProgress.QuizID, userProgress.Score);
            await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = "INSERT INTO \"UserProgress\" (\"UserID\", \"QuizID\", \"Score\", \"CompletionTime\", \"AttemptDate\") VALUES (@UserID, @QuizID, @Score, @CompletionTime, @AttemptDate)";
                await connection.ExecuteAsync(query, userProgress);
            });
        }


        public async Task<bool> ResetPassword(string username, string newPassword)
        {
            _logger.LogInformation("Resetting password for username: {Username}", username);
            var result = await ExecuteWithConnectionAsync(async connection =>
            {
                return await connection.ExecuteAsync(
                    "UPDATE \"User\" SET password = @NewPassword WHERE username = @Username",
                    new { Username = username, NewPassword = newPassword }
                );
            });

            return result > 0;
        }

        public async Task<bool> UpdateProfile(int userId, string username, string password, string languagePreference, float progress)
        {
            _logger.LogInformation("Updating profile for user ID: {UserID}, Username: {Username}", userId, username);
            var result = await ExecuteWithConnectionAsync(async connection =>
            {
                return await connection.ExecuteAsync(
                    "UPDATE \"User\" SET username = @Username, password = @Password, languagePreference = @LanguagePreference, progress = @Progress WHERE userID = @UserId",
                    new { UserId = userId, Username = username, Password = password, LanguagePreference = languagePreference, Progress = progress }
                );
            });

            return result > 0;
        }

        public async Task<bool> TestDatabaseConnection()
        {
            _logger.LogInformation("Testing database connection.");
            try
            {
                using var connection = CreateConnection();
                await Task.Run(() => connection.Open());
                _logger.LogInformation("Database connection successful.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection failed.");
                return false;
            }
        }

        public async Task<IEnumerable<Quiz>> GetQuizzesAsync()
        {
            _logger.LogInformation("Fetching quizzes.");
            return await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = "SELECT * FROM \"Quizzes\"";
                return await connection.QueryAsync<Quiz>(query);
            });
        }

        public async Task<IEnumerable<Question>> GetQuestionsAsync(int quizId)
        {
            _logger.LogInformation("Fetching questions for quiz ID: {QuizID}", quizId);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = "SELECT * FROM \"Questions\" WHERE \"QuizID\" = @QuizID";
                var questions = await connection.QueryAsync<Question>(query, new { QuizID = quizId });

                foreach (var question in questions)
                {
                    question.Choices = new ObservableCollection<string>(question.Choices);
                }

                return questions;
            });
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
