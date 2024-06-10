// App.xaml.cs
using LinguaVerse.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LinguaVerse
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainPage = new NavigationPage(serviceProvider.GetService<LoginPage>());
        }
    }
}
