using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace LinguaVerse
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
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
            if (CardLabel.Text == (BindingContext as ViewModels.FlashcardViewModel).CurrentFlashcard.Question)
            {
                CardLabel.Text = (BindingContext as ViewModels.FlashcardViewModel).CurrentFlashcard.Answer;
            }
            else
            {
                CardLabel.Text = (BindingContext as ViewModels.FlashcardViewModel).CurrentFlashcard.Question;
            }
            await CardFrame.RotateYTo(0, 250, Easing.Linear);
        }
    }
}
