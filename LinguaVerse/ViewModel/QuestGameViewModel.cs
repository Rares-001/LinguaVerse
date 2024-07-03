using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace LinguaVerse
{
    public class QuestGameViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Words { get; set; }
        public ObservableCollection<string> Synonyms { get; set; }
        public ObservableCollection<string> SelectedSynonyms { get; set; }

        private string _selectedWord;
        public string SelectedWord
        {
            get => _selectedWord;
            set
            {
                _selectedWord = value;
                OnPropertyChanged();
            }
        }

        public ICommand CheckCommand { get; }

        private readonly Dictionary<string, List<string>> correctSynonyms = new Dictionary<string, List<string>>()
        {
            {"Happy", new List<string>{ "Joyful", "Cheerful", "Content", "Delighted" }},
            {"Sad", new List<string>{ "Unhappy", "Downcast", "Miserable", "Disheartened" }},
            {"Quick", new List<string>{ "Fast", "Speedy", "Rapid", "Swift" }},
            {"Slow", new List<string>{ "Leisurely", "Sluggish", "Unhurried", "Languid" }},
            {"Bright", new List<string>{ "Luminous", "Radiant", "Vivid", "Brilliant" }},
            {"Dark", new List<string>{ "Dim", "Gloomy", "Murky", "Obscure" }},
            {"Strong", new List<string>{ "Robust", "Sturdy", "Mighty", "Vigorous" }},
            {"Weak", new List<string>{ "Feeble", "Frail", "Fragile", "Delicate" }},
        };

        public QuestGameViewModel()
        {
            Words = new ObservableCollection<string>(correctSynonyms.Keys);
            var shuffledSynonyms = correctSynonyms.Values.SelectMany(s => s).OrderBy(_ => Guid.NewGuid()).ToList();
            Synonyms = new ObservableCollection<string>(shuffledSynonyms);
            SelectedSynonyms = new ObservableCollection<string>();

            CheckCommand = new Command(OnCheck);
        }

        private void OnCheck()
        {
            if (SelectedWord == null)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please select a word.", "OK");
                return;
            }

            if (SelectedSynonyms.Count < 1)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please select at least one synonym.", "OK");
                return;
            }

            var correct = correctSynonyms[SelectedWord];
            var correctMatchCount = SelectedSynonyms.Count(s => correct.Contains(s));

            if (correctMatchCount == SelectedSynonyms.Count && SelectedSynonyms.All(s => correct.Contains(s)))
            {
                Application.Current.MainPage.DisplayAlert("Success", "Correct match!", "OK");
            }
            else
            {
                Application.Current.MainPage.DisplayAlert("Incorrect", "The selected synonyms do not match the selected word. Try again.", "OK");
            }
            SelectedWord = null;
            SelectedSynonyms.Clear();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
 