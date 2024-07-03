using LinguaVerse.ViewModel;

namespace LinguaVerse
{
    public partial class QuestGame : ContentPage
    {
        public QuestGame()
        {
            InitializeComponent();
            BindingContext = new QuestGameViewModel();
        }
    }
}
