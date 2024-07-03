using Dapper;
using LinguaVerse.DAL;
using LinguaVerse.Services;
using LinguaVerse.ViewModel;
using LinguaVerse.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using System.Data;
using Npgsql;

namespace LinguaVerse
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("Arial.otf", "ArialOtf");
                });

            // Register custom type handler
            SqlMapper.AddTypeHandler(new ObservableCollectionTypeHandler());

            // Connection string
            var connectionString = "Host=localhost;Database=LinguaVerseDB;Username=postgres;Password=admin";
            builder.Services.AddSingleton(connectionString); // Register the connection string

            // Register services
            builder.Services.AddSingleton<IDbConnection>(sp => new NpgsqlConnection(connectionString));
            builder.Services.AddTransient<UserRepository>();

            // Register view models
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LanguageSelectionViewModel>();
            builder.Services.AddTransient<FlashcardViewModel>();
            builder.Services.AddTransient<QuizViewModel>();
            builder.Services.AddTransient<Func<int, DashboardViewModel>>(sp => userId => new DashboardViewModel(
                sp.GetRequiredService<UserRepository>(),
                userId,
                sp.GetRequiredService<ILogger<DashboardViewModel>>()
            ));
            builder.Services.AddTransient<Func<int, QuizHistoryViewModel>>(sp => userId => new QuizHistoryViewModel(
                sp.GetRequiredService<UserRepository>(),
                userId
            ));
            builder.Services.AddTransient<Func<UserRepository, DashboardViewModel, int, QuizViewModel>>(sp =>
                (userRepository, dashboardViewModel, userId) => new QuizViewModel(
                    userRepository,
                    dashboardViewModel,
                    userId
                ));
            builder.Services.AddTransient<Func<UserRepository, DashboardViewModel, int, int, TestViewModel>>(sp =>
                (userRepository, dashboardViewModel, userId, testId) => new TestViewModel(
                    userRepository,
                    dashboardViewModel,
                    userId,
                    testId,
                    sp.GetRequiredService<ILogger<TestViewModel>>()
                ));
            builder.Services.AddTransient<Func<UserRepository, int, int, TestViewModelEnglish>>(sp =>
                (userRepository, userId, testId) => new TestViewModelEnglish(
                    userRepository,
                    userId,
                    testId,
                    sp.GetRequiredService<ILogger<TestViewModelEnglish>>()
                ));

            // Register pages
            builder.Services.AddTransient<TestPage1English>();
            builder.Services.AddTransient<TestPage2English>();
            builder.Services.AddTransient<TestPage3English>();
            builder.Services.AddTransient<TestPage4English>();

            builder.Services.AddTransient<TestPage1>();
            builder.Services.AddTransient<TestPage2>();
            builder.Services.AddTransient<TestPage3>();
            builder.Services.AddTransient<TestPage4>();

            builder.Services.AddTransient<QuizPage>();
            builder.Services.AddTransient<QuizPageItalian>(); // Registering QuizPageItalian

            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LanguageSelection>();
            builder.Services.AddTransient<SupportPage>();
            builder.Services.AddTransient(sp =>
            {
                var userId = App.CurrentUserId;
                var dashboardViewModelFactory = sp.GetRequiredService<Func<int, DashboardViewModel>>();
                var dashboardViewModel = dashboardViewModelFactory(userId);
                return new DashboardPage(dashboardViewModel);
            });
            builder.Services.AddTransient<QuizHistoryPage>();
            builder.Services.AddTransient<QuizPage>(sp => new QuizPage(sp.GetRequiredService<Func<UserRepository, DashboardViewModel, int, QuizViewModel>>()(sp.GetRequiredService<UserRepository>(), sp.GetRequiredService<Func<int, DashboardViewModel>>()(App.CurrentUserId), App.CurrentUserId)));
            builder.Services.AddTransient<TestPage1>(sp =>
            {
                var userId = App.CurrentUserId;
                var dashboardViewModelFactory = sp.GetRequiredService<Func<int, DashboardViewModel>>();
                var testViewModelFactory = sp.GetRequiredService<Func<UserRepository, DashboardViewModel, int, int, TestViewModel>>();
                var testViewModel = testViewModelFactory(
                    sp.GetRequiredService<UserRepository>(),
                    dashboardViewModelFactory(userId),
                    userId,
                    1
                );
                return new TestPage1(testViewModel);
            });
            builder.Services.AddTransient<TestPage2>(sp =>
            {
                var userId = App.CurrentUserId;
                var dashboardViewModelFactory = sp.GetRequiredService<Func<int, DashboardViewModel>>();
                var testViewModelFactory = sp.GetRequiredService<Func<UserRepository, DashboardViewModel, int, int, TestViewModel>>();
                var testViewModel = testViewModelFactory(
                    sp.GetRequiredService<UserRepository>(),
                    dashboardViewModelFactory(userId),
                    userId,
                    2
                );
                return new TestPage2(testViewModel);
            });
            builder.Services.AddTransient<TestPage3>(sp =>
            {
                var userId = App.CurrentUserId;
                var dashboardViewModelFactory = sp.GetRequiredService<Func<int, DashboardViewModel>>();
                var testViewModelFactory = sp.GetRequiredService<Func<UserRepository, DashboardViewModel, int, int, TestViewModel>>();
                var testViewModel = testViewModelFactory(
                    sp.GetRequiredService<UserRepository>(),
                    dashboardViewModelFactory(userId),
                    userId,
                    3
                );
                return new TestPage3(testViewModel);
            });
            builder.Services.AddTransient<TestPage4>(sp =>
            {
                var userId = App.CurrentUserId;
                var dashboardViewModelFactory = sp.GetRequiredService<Func<int, DashboardViewModel>>();
                var testViewModelFactory = sp.GetRequiredService<Func<UserRepository, DashboardViewModel, int, int, TestViewModel>>();
                var testViewModel = testViewModelFactory(
                    sp.GetRequiredService<UserRepository>(),
                    dashboardViewModelFactory(userId),
                    userId,
                    4 // Use testId as needed
                );
                return new TestPage4(testViewModel);
            });

            builder.Services.AddTransient<FlashcardsPage>();
            builder.Services.AddTransient<MemoryCardPage>();
            builder.Services.AddTransient<FlashcardViewModel>();

            // Configure logging
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            return builder.Build();
        }
    }
}
