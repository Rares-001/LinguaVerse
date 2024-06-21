using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using LinguaVerse.Views;
using Microsoft.Extensions.Logging;
using LinguaVerse.DAL;

namespace LinguaVerse.ViewModel
{
    public class LanguageSelectionViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<LanguageSelectionViewModel> _logger;

        public ICommand ItalianCommand { get; }
        public ICommand EnglishCommand { get; }
        public ICommand TestCommand { get; }
        public ICommand QuizCommand { get; }
        public ICommand FlashcardsCommand { get; }
        public ICommand NavigateToTestPage1Command { get; }
        public ICommand NavigateToTestPage2Command { get; }
        public ICommand NavigateToTestPage3Command { get; }
        public ICommand NavigateToTestPage4Command { get; }

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

        private bool _isTestOptionsVisible;
        public bool IsTestOptionsVisible
        {
            get => _isTestOptionsVisible;
            set
            {
                _isTestOptionsVisible = value;
                OnPropertyChanged();
            }
        }

        public LanguageSelectionViewModel(IServiceProvider serviceProvider, ILogger<LanguageSelectionViewModel> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            ItalianCommand = new Command(OnItalianClicked);
            EnglishCommand = new Command(OnEnglishClicked);
            TestCommand = new Command(OnTestClicked);
            QuizCommand = new Command(OnQuizClicked);
            FlashcardsCommand = new Command(OnFlashcardsClicked);

            NavigateToTestPage1Command = new Command(() => NavigateToTestPage(1));
            NavigateToTestPage2Command = new Command(() => NavigateToTestPage(2));
            NavigateToTestPage3Command = new Command(() => NavigateToTestPage(3));
            NavigateToTestPage4Command = new Command(() => NavigateToTestPage(4));

            IsOptionsVisible = false;
            IsTestOptionsVisible = false;
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
            IsTestOptionsVisible = true;
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

        private async void NavigateToTestPage(int testId)
        {
            _logger.LogInformation($"Navigating to Test Page {testId}.");
            var testPage = _serviceProvider.GetRequiredService<TestPage1>();

            var testViewModelFactory = _serviceProvider.GetRequiredService<Func<UserRepository, DashboardViewModel, int, int, TestViewModel>>();
            var dashboardViewModelFactory = _serviceProvider.GetRequiredService<Func<int, DashboardViewModel>>();
            var testViewModel = testViewModelFactory(_serviceProvider.GetRequiredService<UserRepository>(), dashboardViewModelFactory(App.CurrentUserId), App.CurrentUserId, testId);
            testPage.BindingContext = testViewModel;

            await Application.Current.MainPage.Navigation.PushAsync(testPage);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
