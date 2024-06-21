using Microsoft.Maui.Controls;
using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class TestPage2English : ContentPage
    {
        private TestViewModelEnglish _viewModel;

        public TestPage2English(TestViewModelEnglish viewModel)
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
