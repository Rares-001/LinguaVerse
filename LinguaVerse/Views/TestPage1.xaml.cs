using LinguaVerse.ViewModel;
using Microsoft.Maui.Controls;

namespace LinguaVerse.Views
{
    public partial class TestPage1 : ContentPage
    {
        private TestViewModel _viewModel;

        public TestPage1(TestViewModel viewModel)
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
