using Microsoft.Maui.Controls;
using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class QuizHistoryPage : ContentPage
    {
        public QuizHistoryPage(QuizHistoryViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
