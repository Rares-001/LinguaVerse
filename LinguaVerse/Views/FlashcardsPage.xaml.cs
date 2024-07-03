using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class FlashcardsPage : ContentPage
    {
        public FlashcardsPage()
        {
            InitializeComponent();
        }

        private async void OnFlipButtonClicked(object sender, EventArgs e)
        {
            await FlipCard();
        }

        private async Task FlipCard()
        {
            await CardFrame.RotateYTo(90, 250, Easing.Linear);
            var viewModel = BindingContext as FlashcardViewModel; 
            if (viewModel != null)
            {
                if (CardLabel.Text == viewModel.CurrentFlashcard.Question)
                {
                    CardLabel.Text = viewModel.CurrentFlashcard.Answer;
                }
                else
                {
                    CardLabel.Text = viewModel.CurrentFlashcard.Question;
                }
            }
            await CardFrame.RotateYTo(0, 250, Easing.Linear);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is FlashcardViewModel viewModel) 
            {
                viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FlashcardViewModel.CurrentFlashcard)) 
            {
                CardLabel.Text = (BindingContext as FlashcardViewModel)?.CurrentFlashcard.Question;
            }
        }

        private async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
