using System;
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

        // Parameterless constructor for XAML
        public QuizViewModel()
        {
        }

        // Constructor for dependency injection
        public QuizViewModel(UserRepository userRepository, DashboardViewModel dashboardViewModel, int userId)
        {
            _userRepository = userRepository;
            _dashboardViewModel = dashboardViewModel;
            _userId = userId;

            LoadQuestionsCommand = new Command(async () => await LoadQuestions());
            CheckAnswersCommand = new Command(CheckAnswers);
            NavigateCommand = new Command(Navigate);

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

        public ICommand LoadQuestionsCommand { get; }
        public ICommand CheckAnswersCommand { get; }
        public ICommand NavigateCommand { get; }

        private async Task LoadQuestions()
        {
            try
            {
                Questions.Clear(); // Clear existing questions before loading new ones

                var quizzes = await _userRepository.GetQuizzesAsync();
                if (quizzes.Any())
                {
                    var quiz = quizzes.First();
                    var questions = await _userRepository.GetQuestionsAsync(quiz.QuizID);
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

        private async void CheckAnswers()
        {
            int correctAnswers = Questions.Count(q => q.Answer == q.SelectedAnswer);
            Points += correctAnswers * 2; // 2 points for each correct answer, missing implementation 
            Result = correctAnswers == Questions.Count ? "Correct" : "Wrong";

            var userProgress = new UserProgress
            {
                UserID = _userId,
                QuizID = Questions.First().QuizID,
                Score = correctAnswers,
                CompletionTime = DateTime.Now.Second,
                AttemptDate = DateTime.Now
            };

            await _userRepository.SaveUserProgressAsync(userProgress);

            // Update the daily streak for today
            string today = DateTime.Now.DayOfWeek.ToString().Substring(0, 3); 
            System.Diagnostics.Debug.WriteLine($"Updating daily streak for today: {today}");
            await _userRepository.UpdateDailyStreak(_userId, today, true);

            // Fetch the updated daily streak from the database and log it
            var updatedDailyStreaks = await _userRepository.GetDailyStreaks(_userId);
            foreach (var streak in updatedDailyStreaks)
            {
                System.Diagnostics.Debug.WriteLine($"Day: {streak.Day}, IsCompleted: {streak.IsCompleted}");
            }

            _dashboardViewModel.UpdateDailyStreaks(updatedDailyStreaks); // Notify DashboardViewModel
        }




        private async void Navigate()
        {
            var nextPage = new Views.SecondPage();
            ((SecondQuizViewModel)nextPage.BindingContext).Points = this.Points;
            await Application.Current.MainPage.Navigation.PushAsync(nextPage);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
