using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class LanguageSelection : ContentPage
    {
        public LanguageSelection()
        {
            InitializeComponent();
            BindingContext = new LanguageSelectionViewModel();
        }
    }
}
