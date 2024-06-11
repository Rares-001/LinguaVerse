using Microsoft.Maui.Controls;
using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class LanguageSelection : ContentPage
    {
        public LanguageSelection()
        {
            InitializeComponent();
            this.BindingContextChanged += OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, System.EventArgs e)
        {
            if (BindingContext is LanguageSelectionViewModel viewModel)
            {
                viewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        private async void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(LanguageSelectionViewModel.IsOptionsVisible) && OptionsSection.IsVisible)
            {
                System.Diagnostics.Debug.WriteLine("Animating options section");
                await OptionsSection.FadeTo(1, 500);
            }
        }
    }
}
