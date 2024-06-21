using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class TestPage1 : ContentPage
    {
        public TestPage1()
        {
            InitializeComponent();
        }

        public TestPage1(TestViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
