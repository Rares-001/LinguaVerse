using Microsoft.Maui.Controls;
using LinguaVerse.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LinguaVerse.Views
{
    public partial class MemoryCardPage : ContentPage
    {
        private MemoryCardViewModel viewModel;
        private Model.MemoryCard firstCard, secondCard;
        private int currentSetIndex = 0;
        private int matchedPairs = 0;
        private readonly Color[] cardColors = { Colors.Red, Colors.Green, Colors.Blue, Colors.Orange, Colors.Purple, Colors.Yellow };

        public MemoryCardPage()
        {
            InitializeComponent();

            viewModel = new MemoryCardViewModel();
            BindingContext = viewModel;

            LoadCardSet(currentSetIndex);
        }

        private void LoadCardSet(int setIndex)
        {
            cardGrid.Children.Clear();
            matchedPairs = 0;

            var cards = viewModel.CardSets[setIndex];
            cards.Shuffle();

            for (int i = 0; i < cards.Count; i++)
            {
                var cardButton = new Button
                {
                    Text = "Flip",
                    BackgroundColor = cardColors[i % cardColors.Length],
                    TextColor = Colors.White,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 24,
                    WidthRequest = 150,
                    HeightRequest = 150
                };
                cardButton.Clicked += OnCardClicked;
                cardButton.BindingContext = cards[i];

                cardGrid.Children.Add(cardButton);
                Grid.SetRow(cardButton, i / 2);
                Grid.SetColumn(cardButton, i % 2);
            }
        }

        private async void OnCardClicked(object sender, EventArgs e)
        {
            var button = sender as Button;
            var card = button.BindingContext as Model.MemoryCard;

            if (firstCard == null)
            {
                firstCard = card;
                button.Text = card.Text;
                button.IsEnabled = false;
            }
            else if (secondCard == null)
            {
                secondCard = card;
                button.Text = card.Text;
                button.IsEnabled = false;

                if (firstCard.Match == secondCard.Text)
                {
                    await DisplayAlert("Match", "You've found a match!", "Next");

                    foreach (var btn in cardGrid.Children.OfType<Button>())
                    {
                        if (btn.Text != firstCard.Text && btn.Text != secondCard.Text)
                        {
                            btn.Text = "Flip";
                            btn.IsEnabled = true;
                        }
                    }

                    firstCard = secondCard = null;
                    matchedPairs++;

                    if (matchedPairs * 2 == viewModel.CardSets[currentSetIndex].Count)
                    {
                        await Task.Delay(1000);
                        ProceedToNextSet();
                    }
                }
                else
                {
                    await Task.Delay(1000);

                    foreach (var btn in cardGrid.Children.OfType<Button>())
                    {
                        if (btn.BindingContext == firstCard || btn.BindingContext == secondCard)
                        {
                            btn.Text = "Flip";
                            btn.IsEnabled = true;
                        }
                    }

                    firstCard = secondCard = null;
                }
            }
        }

        private void ProceedToNextSet()
        {
            currentSetIndex++;
            if (currentSetIndex < viewModel.CardSets.Count)
            {
                LoadCardSet(currentSetIndex);
            }
            else
            {
                DisplayAlert("Congratulations", "You've completed all the card sets!", "OK");
            }
        }

        private async void OnCloseButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
