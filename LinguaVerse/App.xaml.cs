using LinguaVerse.Views;

namespace LinguaVerse
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new DashboardPage();
        }
    }
}
