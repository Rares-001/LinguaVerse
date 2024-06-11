using Dapper;
using LinguaVerse.DAL;
using LinguaVerse.Services;
using LinguaVerse.ViewModel;
using LinguaVerse.Views;
using Microsoft.Extensions.DependencyInjection;
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

            // Configure DI
            var connectionString = "Host=localhost;Database=LinguaVerseDB;Username=postgres;Password=admin";
            builder.Services.AddSingleton<IDbConnection>(sp => new NpgsqlConnection(connectionString));
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<LanguageSelectionViewModel>();

            // Register QuizPage with dependencies
            builder.Services.AddTransient<QuizPage>(sp =>
            {
                var userRepository = sp.GetRequiredService<UserRepository>();
                var userId = 1; 
                return new QuizPage(new QuizViewModel(userRepository, userId));
            });

            // Register DashboardPage with dependencies
            builder.Services.AddTransient<DashboardPage>(sp =>
            {
                var userRepository = sp.GetRequiredService<UserRepository>();
                var userId = 1; 
                return new DashboardPage(new DashboardViewModel(userRepository, userId));
            });

            return builder.Build();
        }
    }
}
