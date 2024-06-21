using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class TestPage2 : ContentPage
    {
        public TestPage2()
        {
            InitializeComponent();
        }

        public TestPage2(TestViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
