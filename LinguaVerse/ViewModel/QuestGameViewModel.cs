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

        private string _selectedSynonym;
        public string SelectedSynonym
        {
            get => _selectedSynonym;
            set
            {
                _selectedSynonym = value;
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

            CheckCommand = new Command(OnCheck);
        }

        private async void OnCheck()
        {
            if (SelectedWord == null || SelectedSynonym == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Please select a word and a synonym.", "OK");
                return;
            }

            var correct = correctSynonyms[SelectedWord];
            bool isMatch = correct.Contains(SelectedSynonym);

            if (isMatch)
            {
                await Application.Current.MainPage.DisplayAlert("Match Result", "The selected synonym matches the selected word!", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Match Result", "The selected synonym does not match the selected word.", "OK");
            }
            SelectedWord = null;
            SelectedSynonym = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
