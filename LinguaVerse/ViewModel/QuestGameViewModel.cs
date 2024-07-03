using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace LinguaVerse.ViewModel
{
    public class QuestGameViewModel : BindableObject
    {
        public ObservableCollection<string> Words { get; set; }
        public ObservableCollection<string> SynonymOptions { get; set; }
        public ObservableCollection<string> SelectedSynonyms { get; set; }
        public ICommand CheckCommand { get; }

        public QuestGameViewModel()
        {
            Words = new ObservableCollection<string>
            {
                "Happy",
                "Sad",
                "Quick",
                "Slow"
            };

            SynonymOptions = new ObservableCollection<string>
            {
                "Joyful",
                "Unhappy",
                "Fast",
                "Sluggish",
                "Cheerful",
                "Downcast",
                "Speedy",
                "Leisurely"
            };

            SelectedSynonyms = new ObservableCollection<string>();
            CheckCommand = new Command(CheckAnswers);
        }

        private void CheckAnswers()

        {

            var correctSynonyms = new List<string> { "Joyful", "Cheerful", "Unhappy", "Downcast", "Fast", "Speedy", "Sluggish", "Leisurely" };
            bool isCorrect = SelectedSynonyms.All(s => correctSynonyms.Contains(s)) && SelectedSynonyms.Count == correctSynonyms.Count;

            if (isCorrect)
            {
                Application.Current.MainPage.DisplayAlert("Correct!", "All synonyms are correctly selected.", "OK");
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Incorrect", "Some synonyms are incorrect. Try again.", "OK");
            }
        }
    }
}
