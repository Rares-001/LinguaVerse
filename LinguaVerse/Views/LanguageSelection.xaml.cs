using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class LanguageSelection : ContentPage
    {
        public LanguageSelection()
        {
            InitializeComponent();

            var viewModel = App.Services.GetService<LanguageSelectionViewModel>();
            BindingContext = viewModel;
        }
    }
}
