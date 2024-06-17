using Microsoft.Maui.Controls;
using LinguaVerse.ViewModel;
using System.Linq;
using System.Threading.Tasks;

namespace LinguaVerse.Views
{
    public partial class MainPage : ContentPage
    {
        private MemoryCardViewModel viewModel;
        private Model.MemoryCard firstCard, secondCard;
        private readonly Color[] cardColors = { Colors.Red, Colors.Green, Colors.Blue, Colors.Orange };

        public MainPage()
        {
            InitializeComponent();

            viewModel = new MemoryCardViewModel();
            BindingContext = viewModel;
            for (int i = 0; i < viewModel.Cards.Count; i++)
            {
                var cardButton = new Button
                {
                    Text = "Flip",
                    BackgroundColor = cardColors[i],
                    TextColor = Colors.White,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 24,
                    WidthRequest = 150,
                    HeightRequest = 150
                };
                cardButton.Clicked += OnCardClicked;
                cardButton.BindingContext = viewModel.Cards[i];

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
                        btn.Text = "Flip";
                        btn.IsEnabled = true;
                    }

                    firstCard = secondCard = null;
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
    }
}
