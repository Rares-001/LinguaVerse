using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using LinguaVerse.DAL;
using LinguaVerse.Model;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using System.Linq;

namespace LinguaVerse.ViewModel
{
    public class TestViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        private readonly DashboardViewModel _dashboardViewModel;
        private readonly int _userId;
        private readonly int _quizId;
        private readonly ILogger<TestViewModel> _logger;

        public TestViewModel(UserRepository userRepository, DashboardViewModel dashboardViewModel, int userId, int quizId, ILogger<TestViewModel> logger)
        {
            _userRepository = userRepository;
            _dashboardViewModel = dashboardViewModel;
            _userId = userId;
            _quizId = quizId;
            _logger = logger;

            LoadQuestionsCommand = new Command(async () => await LoadQuestions());
            SubmitTestCommand = new Command(SubmitTest);

            LoadQuestions(); // Load questions initially
        }

        // Parameterless constructor for XAML compatibility
        public TestViewModel() { }

        private ObservableCollection<Question> _questions = new ObservableCollection<Question>();
        public ObservableCollection<Question> Questions
        {
            get => _questions;
            set
            {
                _questions = value;
                OnPropertyChanged();
            }
        }

        private int _score = 0;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadQuestionsCommand { get; }
        public ICommand SubmitTestCommand { get; }

        private async Task LoadQuestions()
        {
            try
            {
                _logger.LogInformation("Loading questions for test.");
                var questions = (await _userRepository.GetQuestionsForTestAsync(_quizId)).ToList();
                if (questions.Any())
                {
                    Shuffle(questions);
                    Questions = new ObservableCollection<Question>(questions);
                    _logger.LogInformation($"Loaded {Questions.Count} questions for test.");
                }
                else
                {
                    _logger.LogWarning($"No questions found for quiz ID: {_quizId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading questions: {ex.Message}");
            }
        }

        private void Shuffle<T>(IList<T> list)
        {
            var rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private async void SubmitTest()
        {
            foreach (var question in Questions)
            {
                question.IsCorrect = question.Answer == question.SelectedAnswer;
            }

            int correctAnswers = Questions.Count(q => q.IsCorrect == true);
            int score = correctAnswers * 2; // 2 points for each correct answer

            var userTestProgress = new UserTestProgress
            {
                UserID = _userId,
                TestID = _quizId,
                Score = score,
                CompletionTime = DateTime.Now.Second,
                AttemptDate = DateTime.Now
            };

            await _userRepository.SaveUserTestProgressAsync(userTestProgress);

            float averageScore = await _userRepository.GetUserTestProgressAsync(_userId);
            await _userRepository.UpdateProfile(_userId, null, null, null, averageScore);

            _dashboardViewModel.LoadUserData(); // Refresh the dashboard data

            await Application.Current.MainPage.DisplayAlert("Test Completed", $"You scored {score} points", "OK");
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
