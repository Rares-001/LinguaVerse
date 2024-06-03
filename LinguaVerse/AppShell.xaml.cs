using LinguaVerse.Views;

namespace LinguaVerse
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SecondPage), typeof(SecondPage));
        }
    }
}