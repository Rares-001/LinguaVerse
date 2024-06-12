using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using LinguaVerse.DAL;
using LinguaVerse.ViewModel;
using LinguaVerse.Views;
using Npgsql;
using System.Data;
using LinguaVerse.Seeders;

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

            var connectionString = "Host=localhost;Database=LinguaVerseDBBBBB;Username=postgres;Password=admin";

            // Configure DI
            builder.Services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(connectionString));
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<DashboardPage>();

            return builder.Build();
        }
    }
}
