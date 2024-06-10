using LinguaVerse.ViewModel;
using Microsoft.Maui.Controls;

namespace LinguaVerse.Views
{
    public partial class SecondPage : ContentPage
    {
        public SecondPage()
        {
            InitializeComponent();
            BindingContext = new SecondQuizViewModel(); 
        }
    }
}
