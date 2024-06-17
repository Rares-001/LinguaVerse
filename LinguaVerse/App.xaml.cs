using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace LinguaVerse
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new Views.MainPage());
        }
    }
}
