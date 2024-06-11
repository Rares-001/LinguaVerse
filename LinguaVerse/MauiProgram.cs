using Microsoft.Extensions.Logging;

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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

<<<<<<< Updated upstream
#if DEBUG
    		builder.Logging.AddDebug();
#endif
=======
            // Configure DI
            var connectionString = "Host=localhost;Database=LinguaVerse;Username=postgres;Password=Polly55";
            builder.Services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(connectionString));
            builder.Services.AddTransient<UserRepository>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<DashboardPage>();
            builder.Services.AddTransient<DataSeeder>();
>>>>>>> Stashed changes

            return builder.Build();
        }
    }
}
