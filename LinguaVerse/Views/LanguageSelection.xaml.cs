using Microsoft.Maui.Controls;

namespace LinguaVerse.Views
{
    public partial class LanguageSelection : ContentPage
    {
        public LanguageSelection()
        {
            InitializeComponent();
        }

        private async void OnButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                await button.ScaleTo(1.2, 100);
                await button.ScaleTo(1, 100);
            }
        }
    }
}
