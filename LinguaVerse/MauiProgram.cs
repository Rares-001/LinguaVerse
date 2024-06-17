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
                });

            // Register custom type handler
            SqlMapper.AddTypeHandler(new ObservableCollectionTypeHandler());

            // Connection string
            var connectionString = "Host=localhost;Database=LinguaVerseDB;Username=postgres;Password=admin";
            builder.Services.AddSingleton(connectionString); // Register the connection string

            // Register services
            builder.Services.AddSingleton<IDbConnection>(sp => new NpgsqlConnection(connectionString));
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<LanguageSelectionViewModel>();
            builder.Services.AddTransient<LanguageSelection>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<Func<int, DashboardViewModel>>(sp => userId => new DashboardViewModel(sp.GetRequiredService<UserRepository>(), userId));
            builder.Services.AddTransient<Func<UserRepository, DashboardViewModel, int, QuizViewModel>>(sp =>
                (userRepository, dashboardViewModel, userId) => new QuizViewModel(userRepository, dashboardViewModel, userId));
            builder.Services.AddTransient<QuizPage>(sp => new QuizPage(sp.GetRequiredService<Func<UserRepository, DashboardViewModel, int, QuizViewModel>>()(sp.GetRequiredService<UserRepository>(), sp.GetRequiredService<Func<int, DashboardViewModel>>()(App.CurrentUserId), App.CurrentUserId)));

            // Configure logging
            builder.Logging.AddConsole();
            builder.Logging.AddDebug();

            return builder.Build();
        }
    }
}
