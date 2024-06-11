using LinguaVerse.Views;
using Microsoft.Extensions.DependencyInjection;
using LinguaVerse.Seeders;

namespace LinguaVerse
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainPage = new NavigationPage(serviceProvider.GetService<LoginPage>());

            SeedDatabase(serviceProvider);
        }

        private async void SeedDatabase(IServiceProvider serviceProvider)
        {
            var dataSeeder = serviceProvider.GetService<DataSeeder>();
            await dataSeeder.SeedDataAsync();
        }
    }
}
