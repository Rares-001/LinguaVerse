using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using LinguaVerse.Views;

namespace LinguaVerse.ViewModel
{
    public class LanguageSelectionViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;

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

        // Parameterless constructor for XAML compatibility
        public LanguageSelectionViewModel()
        {
        }

        // Constructor for dependency injection
        public LanguageSelectionViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

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
            // Implementation missing
        }

        private async void OnQuizClicked()
        {
            var quizPage = _serviceProvider.GetRequiredService<QuizPage>();
            await Application.Current.MainPage.Navigation.PushAsync(quizPage);
        }

        private async void OnFlashcardsClicked()
        {
            // Implementation missing
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
