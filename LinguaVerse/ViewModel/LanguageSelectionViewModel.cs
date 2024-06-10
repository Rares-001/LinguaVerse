using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace LinguaVerse.ViewModel
{
    public class LanguageSelectionViewModel : INotifyPropertyChanged
    {
        public ICommand ItalianCommand { get; }
        public ICommand EnglishCommand { get; }
        public ICommand TestCommand { get; }
        public ICommand QuizCommand { get; }
        public ICommand FlashcardsCommand { get; }

        private bool _isOptionsVisible;
        public bool IsOptionsVisible
        {
            get => _isOptionsVisible;
            set
            {
                _isOptionsVisible = value;
                OnPropertyChanged();
            }
        }

        public LanguageSelectionViewModel()
        {
            ItalianCommand = new Command(OnItalianClicked);
            EnglishCommand = new Command(OnEnglishClicked);
            TestCommand = new Command(OnTestClicked);
            QuizCommand = new Command(OnQuizClicked);
            FlashcardsCommand = new Command(OnFlashcardsClicked);

            IsOptionsVisible = false;
        }

        private async void OnItalianClicked()
        {
            await ShowOptionsAsync();
        }

        private async void OnEnglishClicked()
        {
            await ShowOptionsAsync();
        }

        private async Task ShowOptionsAsync()
        {

            await Task.Delay(300); 
            IsOptionsVisible = true;
        }

        private async void OnTestClicked()
        {
            
        }

        private async void OnQuizClicked()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.QuizPage());
        }

        private async void OnFlashcardsClicked()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
