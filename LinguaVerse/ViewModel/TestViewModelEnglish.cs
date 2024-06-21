using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using LinguaVerse.DAL;
using LinguaVerse.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;

namespace LinguaVerse.ViewModel
{
    public class TestViewModelEnglish : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger<TestViewModelEnglish> _logger;
        private int _userId;
        private int _testId;

        // Parameterless constructor for XAML design-time support
        public TestViewModelEnglish()
        {
            Questions = new ObservableCollection<Question>();
        }

        // Constructor initializes the repository, user ID, and test ID
        public TestViewModelEnglish(UserRepository userRepository, int userId, int testId, ILogger<TestViewModelEnglish> logger)
        {
            _userRepository = userRepository;
            _userId = userId;
            _testId = testId;
            _logger = logger;
            Questions = new ObservableCollection<Question>();

            SubmitTestCommand = new Command(async () => await SubmitTestAsync());
            LoadQuestionsCommand = new Command(async () => await LoadQuestionsAsync());
        }

        public ObservableCollection<Question> Questions { get; }

        public ICommand SubmitTestCommand { get; }
        public ICommand LoadQuestionsCommand { get; }

        // Loads the questions for the test
        private async Task LoadQuestionsAsync()
        {
            var questions = await _userRepository.GetQuestionsAsync(_testId);
            Questions.Clear();
            foreach (var question in questions)
            {
                Questions.Add(question);
            }
        }

        // Submits the test and calculates the score
        private async Task SubmitTestAsync()
        {
            int score = 0;
            foreach (var question in Questions)
            {
                if (question.SelectedAnswer == question.Answer)
                {
                    score++;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
