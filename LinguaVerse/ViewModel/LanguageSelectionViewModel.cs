using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace LinguaVerse.ViewModel
{
    public class LanguageSelectionViewModel : INotifyPropertyChanged
    {
        public ICommand ItalianCommand { get; }
        public ICommand EnglishCommand { get; }

        public LanguageSelectionViewModel()
        {
            ItalianCommand = new Command(OnItalianClicked);
            EnglishCommand = new Command(OnEnglishClicked);
        }

        private void OnItalianClicked()
        {
            // to add logic
        }

        private void OnEnglishClicked()
        {
            // to add logic
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
