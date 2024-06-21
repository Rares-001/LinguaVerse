using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class TestPage4 : ContentPage
    {
        public TestPage4()
        {
            InitializeComponent();
        }

        public TestPage4(TestViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
