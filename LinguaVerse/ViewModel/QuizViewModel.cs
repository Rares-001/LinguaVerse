using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using LinguaVerse.DAL;
using LinguaVerse.Model;
using Microsoft.Maui.Controls;

namespace LinguaVerse.ViewModel
{
    public class QuizViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        private readonly DashboardViewModel _dashboardViewModel;
        private readonly int _userId;

        // Default constructor for XAML design-time support
        public QuizViewModel() { }

        // Constructor initializes the repository, dashboard view model, and user ID
        public QuizViewModel(UserRepository userRepository, DashboardViewModel dashboardViewModel, int userId)
        {
            _userRepository = userRepository;
            _dashboardViewModel = dashboardViewModel;
            _userId = userId;

            LoadQuestionsCommand = new Command(async () => await LoadQuestions());
            CheckAnswersCommand = new Command(CheckAnswers);

            LoadQuestions(); // Load questions initially
        }

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

        private string _result = string.Empty;
        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
                IsResultVisible = true;
            }
        }

        private int _points = 0;
        public int Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }

        private bool _isResultVisible = false;
        public bool IsResultVisible
        {
            get => _isResultVisible;
            set
            {
                _isResultVisible = value;
                OnPropertyChanged();
            }
        }

        private bool _showResults = false;
        public bool ShowResults
        {
            get => _showResults;
            set
            {
                _showResults = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadQuestionsCommand { get; }
        public ICommand CheckAnswersCommand { get; }

        // Loads the questions for the quiz
        private async Task LoadQuestions()
        {
            try
            {
                Questions.Clear(); // Clear existing questions before loading new ones
                ShowResults = false; // Reset the results visibility

                var quizzes = await _userRepository.GetQuizzesAsync();
                if (quizzes.Any())
                {
                    var quiz = quizzes.First();
                    var questions = await _userRepository.GetQuestionsAsync(quiz.QuizID); // Removed the language parameter
                    foreach (var question in questions)
                    {
                        question.IsCorrect = null; // Reset the state of each question
                    }
                    Questions = new ObservableCollection<Question>(questions);
                    System.Diagnostics.Debug.WriteLine($"Fetched {Questions.Count} questions from database.");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No quizzes found in database.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading questions: {ex.Message}");
            }
        }

        // Checks the answers provided by the user
        private async void CheckAnswers()
        {
            foreach (var question in Questions)
            {
                question.IsCorrect = question.Answer == question.SelectedAnswer;
                System.Diagnostics.Debug.WriteLine($"Question: {question.QuestionText}, Selected: {question.SelectedAnswer}, Correct: {question.IsCorrect}");
            }

            int correctAnswers = Questions.Count(q => q.IsCorrect == true);
            int score = correctAnswers * 2; // 2 points for each correct answer
            Points = score;  // Update the points property
            Result = correctAnswers == Questions.Count ? "Correct" : "Wrong";

            var userProgress = new UserProgress
            {
                UserID = _userId,
                QuizID = Questions.First().QuizID,
                Score = score,
                CompletionTime = DateTime.Now.Second,
                AttemptDate = DateTime.Now
            };

            await _userRepository.SaveUserProgressAsync(userProgress);

            // Update the daily streak for today
            string today = DateTime.Now.DayOfWeek.ToString().Substring(0, 3);
            await _userRepository.UpdateDailyStreak(_userId, today, true);

            var dailyStreaks = await _userRepository.GetDailyStreaks(_userId);
            _dashboardViewModel.UpdateDailyStreaks(dailyStreaks); // Notify DashboardViewModel

            ShowResults = true; // Show the results after checking answers
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
