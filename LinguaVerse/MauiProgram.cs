using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
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


#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // Configure DI
            var connectionString = "Host=localhost;Database=LinguaVerse;Username=postgres;Password=Polly55";

            // Configure DI
            var connectionString = "Host=localhost;Database=LinguaVerseDB;Username=postgres;Password=admin";
            builder.Services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(connectionString));
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<DataSeeder>();

            return builder.Build();
        }
    }
}

