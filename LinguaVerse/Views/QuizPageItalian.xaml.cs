using LinguaVerse.ViewModel;
using Microsoft.Maui.Controls;

namespace LinguaVerse.Views
{
    public partial class QuizPageItalian : ContentPage
    {
        private TestViewModel _viewModel;

        public QuizPageItalian(TestViewModel viewModel)
        {
            InitializeComponent();  
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadQuestionsCommand.Execute(null);
        }
    }
}
