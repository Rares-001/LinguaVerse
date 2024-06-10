// MauiProgram.cs
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using LinguaVerse.DAL;
using LinguaVerse.ViewModel;
using LinguaVerse.Views;

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

            // Configure DI
            var connectionString = "Host=localhost;Database=LinguaVerseDB;Username=postgres;Password=admin";
            builder.Services.AddSingleton(new Database(connectionString));
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();

            var mauiApp = builder.Build();
            var serviceProvider = mauiApp.Services;
            return mauiApp;
        }
    }
}
