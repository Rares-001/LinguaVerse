using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            this.Appearing += LoginPage_Appearing;
            BindingContext = viewModel;
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100, Easing.CubicIn);
            await button.ScaleTo(1.0, 100, Easing.CubicOut);
        }

        private async void OnForgotPasswordTapped(object sender, EventArgs e)
        {
            await DisplayAlert("Forgot Password", "Password recovery is not implemented yet.", "OK");
        }

        private async void LoginPage_Appearing(object sender, EventArgs e)
        {
            AnimateGradientBackground();
            await AnimateTitle();
        }

        private async Task AnimateTitle()
        {
            while (true)
            {
                await TitleLabel.TranslateTo(10, 0, 1000, Easing.Linear);
                await TitleLabel.TranslateTo(-10, 0, 1000, Easing.Linear);
            }
        }

        private void AnimateGradientBackground()
        {
            var animation = new Animation();

            var gradientStop1 = ((LinearGradientBrush)Background).GradientStops[0];
            var gradientStop2 = ((LinearGradientBrush)Background).GradientStops[1];

            animation.Add(0, 0.5, new Animation(v => gradientStop1.Color = Color.FromRgba(v, 0.1, 0.6, 1), 0.3, 1));
            animation.Add(0, 0.5, new Animation(v => gradientStop2.Color = Color.FromRgba(0.6, v, 0.1, 1), 0.3, 1));

            animation.Add(0.5, 1, new Animation(v => gradientStop1.Color = Color.FromRgba(0.3, v, 1, 1), 1, 0.3));
            animation.Add(0.5, 1, new Animation(v => gradientStop2.Color = Color.FromRgba(v, 0.6, 0.1, 1), 1, 0.3));

            animation.Commit(this, "GradientAnimation", length: 10000, repeat: () => true);
        }
    }
}
