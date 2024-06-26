﻿using Dapper;
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

        // Constructor to initialize the repository with connection string and logger
        public UserRepository(string connectionString, ILogger<UserRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        // Method to create a new database connection
        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        // Generic method to execute a database query and return a result
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

        // Generic method to execute a database command without returning a result
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

        // Method to register a new user
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

                    // Initialize course progress for the new user
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

        // Method to log in a user
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

        // Method to get user ID by username
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

        // Method to get user details by username
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

        // Method to get user details by user ID
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

        // Method to get daily streaks for a user
        public async Task<IEnumerable<DailyStreak>> GetDailyStreaks(int userId)
        {
            using var connection = CreateConnection();
            const string query = "SELECT streakid AS StreakId, userid AS UserId, day, completed AS IsCompleted FROM \"DailyStreaks\" WHERE userid = @UserId";
            return await connection.QueryAsync<DailyStreak>(query, new { UserId = userId });
        }

        // Method to update daily streak for a user
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

        // Method to get course progress for a user
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

        // Method to get featured courses
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

        // Method to get user progress
        public async Task<IEnumerable<UserProgress>> GetUserProgressAsync(int userId)
        {
            _logger.LogInformation("Fetching user progress for user ID: {UserID}", userId);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = "SELECT * FROM \"UserProgress\" WHERE \"UserID\" = @UserId";
                return await connection.QueryAsync<UserProgress>(query, new { UserId = userId });
            });
        }

        // Method to save user progress
        public async Task SaveUserProgressAsync(UserProgress userProgress)
        {
            _logger.LogInformation("Saving user progress for user ID: {UserID}, Quiz ID: {QuizID}, Score: {Score}", userProgress.UserID, userProgress.QuizID, userProgress.Score);
            await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = "INSERT INTO \"UserProgress\" (\"UserID\", \"QuizID\", \"Score\", \"CompletionTime\", \"AttemptDate\") VALUES (@UserID, @QuizID, @Score, @CompletionTime, @AttemptDate)";
                await connection.ExecuteAsync(query, userProgress);
            });
        }

        // Method to reset password for a user
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

        // Method to update user profile
        public async Task<bool> UpdateProfile(int userId, string username, string password, string languagePreference, float? progress)
        {
            _logger.LogInformation("Updating profile for user ID: {UserID}, Username: {Username}", userId, username);
            var result = await ExecuteWithConnectionAsync(async connection =>
            {
                // Fetch current values to avoid null constraint violations
                var currentProfile = await GetUserById(userId);
                var updatedUsername = username ?? currentProfile.Username;
                var updatedPassword = password ?? currentProfile.Password;
                var updatedLanguagePreference = languagePreference ?? currentProfile.LanguagePreference;
                var updatedProgress = progress ?? currentProfile.Progress;

                return await connection.ExecuteAsync(
                    "UPDATE \"User\" SET username = @Username, password = @Password, languagePreference = @LanguagePreference, progress = @Progress WHERE userID = @UserId",
                    new { UserId = userId, Username = updatedUsername, Password = updatedPassword, LanguagePreference = updatedLanguagePreference, Progress = updatedProgress }
                );
            });

            return result > 0;
        }

        // Method to test database connection
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

        // Method to fetch all quizzes
        public async Task<IEnumerable<Quiz>> GetQuizzesAsync()
        {
            _logger.LogInformation("Fetching quizzes.");
            return await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = "SELECT * FROM \"Quizzes\"";
                return await connection.QueryAsync<Quiz>(query);
            });
        }

        // Method to fetch questions for a specific quiz
        public async Task<IEnumerable<Question>> GetQuestionsAsync(int quizId)
        {
            _logger.LogInformation("Fetching questions for quiz ID: {QuizID}", quizId);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                string query = "SELECT * FROM \"Questions\" WHERE \"QuizID\" = @QuizID";
                var questions = await connection.QueryAsync<Question>(query, new { QuizID = quizId });

                foreach (var question in questions)
                {
                    question.Choices = new ObservableCollection<string>(question.Choices);
                }

                return questions;
            });
        }

        // Method to fetch questions for a specific test
        public async Task<IEnumerable<Question>> GetQuestionsForTestAsync(int quizId)
        {
            _logger.LogInformation("Fetching questions for quiz ID: {QuizID}", quizId);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = @"
                    SELECT 
                        ""QuestionID"", 
                        ""QuestionText"", 
                        ""Answer"", 
                        ""Choices"", 
                        ""explanation"", 
                        ""hint"" 
                    FROM ""Questions"" 
                    WHERE ""QuizID"" = @QuizID";
                var questions = await connection.QueryAsync<Question>(query, new { QuizID = quizId });
                _logger.LogInformation($"Retrieved {questions.Count()} questions for quiz ID: {quizId}");
                return questions;
            });
        }

        // Method to save user test progress
        public async Task SaveUserTestProgressAsync(UserTestProgress userTestProgress)
        {
            _logger.LogInformation("Saving user test progress for user ID: {UserID}, Test ID: {TestID}, Score: {Score}", userTestProgress.UserID, userTestProgress.TestID, userTestProgress.Score);
            await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = "INSERT INTO \"UserTestProgress\" (\"UserID\", \"TestID\", \"Score\", \"CompletionTime\", \"AttemptDate\") VALUES (@UserID, @TestID, @Score, @CompletionTime, @AttemptDate)";
                await connection.ExecuteAsync(query, userTestProgress);
            });
        }

        // Method to fetch user test progress
        public async Task<float> GetUserTestProgressAsync(int userId)
        {
            _logger.LogInformation("Fetching user test progress for user ID: {UserID}", userId);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                const string query = "SELECT AVG(\"Score\") FROM \"UserTestProgress\" WHERE \"UserID\" = @UserId";
                return await connection.ExecuteScalarAsync<float>(query, new { UserId = userId });
            });
        }

        // Method to fetch completed tests count
        public async Task<int> GetCompletedTestsCount(int userId)
        {
            _logger.LogInformation("Fetching completed tests count for user ID: {UserID}", userId);
            const string query = "SELECT COUNT(*) FROM \"UserTestProgress\" WHERE \"UserID\" = @UserId AND \"Score\" > 0";
            int completedTestsCount = await ExecuteWithConnectionAsync(async connection =>
            {
                return await connection.ExecuteScalarAsync<int>(query, new { UserId = userId });
            });

            System.Diagnostics.Debug.WriteLine($"Completed Tests Count from DB: {completedTestsCount}");

            return completedTestsCount;
        }

        // Method to calculate user progress based on completed tests
        public async Task<float> CalculateUserProgressAsync(int userId)
        {
            _logger.LogInformation($"Fetching completed tests count for user ID: {userId}");
            int completedTestsCount = await GetCompletedTestsCount(userId);
            _logger.LogInformation($"Completed Tests Count from DB: {completedTestsCount}");

            int totalTests = 4; // Assuming there are 4 tests in total
            float progressPercentage = (float)completedTestsCount / totalTests;
            _logger.LogInformation($"Raw Progress Percentage: {progressPercentage}");

            // Cap the progress percentage at 100%
            if (progressPercentage > 1)
            {
                progressPercentage = 1;
            }

            _logger.LogInformation($"Capped Progress Percentage: {progressPercentage}");
            return progressPercentage;
        }

        // Method to fetch questions for a specific quiz
        public async Task<IEnumerable<Question>> GetQuestionsForQuizAsync(int quizId)
        {
            _logger.LogInformation("Fetching questions for quiz ID: {QuizID}", quizId);
            return await ExecuteWithConnectionAsync(async connection =>
            {
                string query = "SELECT * FROM \"Questions\" WHERE \"QuizID\" = @QuizID";
                var questions = await connection.QueryAsync<Question>(query, new { QuizID = quizId });

                foreach (var question in questions)
                {
                    question.Choices = new ObservableCollection<string>(question.Choices);
                }

                return questions;
            });
        }
    }

    // User class representing a user entity
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LanguagePreference { get; set; }
        public float Progress { get; set; }
    }

    // CourseProgress class representing the progress of a course for a user
    public class CourseProgress
    {
        public string CourseName { get; set; }
        public float Progress { get; set; }
        public string Level { get; set; }
    }

    // FeaturedCourse class representing a featured course
    public class FeaturedCourse
    {
        public string CourseName { get; set; }
        public int Duration { get; set; }
        public int Questions { get; set; }
        public string Level { get; set; }
        public string FlagIcon { get; set; }
    }
}
