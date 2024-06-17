using Microsoft.Maui.Controls;
using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class QuizPage : ContentPage
    {
        public QuizPage(QuizViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        // Default constructor for XAML preview
        public QuizPage()
        {
            InitializeComponent();
            BindingContext = new QuizViewModel();
        }
    }
}
