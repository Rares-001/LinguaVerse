using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public ICommand CheckCommand { get; }

        public QuestGameViewModel()
        {
            Words = new ObservableCollection<string> { "Happy", "Sad", "Quick", "Slow" };
            Synonyms = new ObservableCollection<string> { "Joyful", "Unhappy", "Fast", "Sluggish", "Cheerful", "Downcast", "Speedy", "Leisurely" };
            SelectedSynonyms = new ObservableCollection<string>();

            CheckCommand = new Command(OnCheck);
        }

        private void OnCheck()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
