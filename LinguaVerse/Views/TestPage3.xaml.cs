using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class TestPage3 : ContentPage
    {
        public TestPage3()
        {
            InitializeComponent();
        }

        public TestPage3(TestViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
