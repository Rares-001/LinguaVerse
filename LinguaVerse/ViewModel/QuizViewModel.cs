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
        private readonly int _userId;

        // Parameterless constructor for XAML
        public QuizViewModel()
        {
        }

        // Constructor for dependency injection
        public QuizViewModel(UserRepository userRepository, int userId)
        {
            _userRepository = userRepository;
            _userId = userId;

            LoadQuestions();
            CheckAnswersCommand = new Command(CheckAnswers);
            NavigateCommand = new Command(Navigate);
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

        public ICommand CheckAnswersCommand { get; }
        public ICommand NavigateCommand { get; }

        private async void LoadQuestions()
        {
            var quizzes = await _userRepository.GetQuizzesAsync();
            if (quizzes.Any())
            {
                var questions = await _userRepository.GetQuestionsAsync(quizzes.First().QuizID);
                Questions = new ObservableCollection<Question>(questions);

                // Debugging output
                System.Diagnostics.Debug.WriteLine($"Fetched {Questions.Count} questions from database.");
            }
            else
            {
                // Debugging output
                System.Diagnostics.Debug.WriteLine("No quizzes found in database.");
            }
        }

        private async void CheckAnswers()
        {
            int correctAnswers = Questions.Count(q => q.Answer == q.SelectedAnswer);
            Points += correctAnswers * 2; // 2 points for each correct answer
            Result = correctAnswers == Questions.Count ? "Correct" : "Wrong";

            // Save user progress
            var userProgress = new DAL.UserProgress
            {
                UserID = _userId,
                QuizID = Questions.First().QuizID,
                Score = correctAnswers,
                CompletionTime = DateTime.Now.Second, // TO BE REMOVED
                AttemptDate = DateTime.Now // TO BE REMOVED
            };

            await _userRepository.SaveUserProgressAsync(userProgress);

            // Update daily streak
            string today = DateTime.Now.DayOfWeek.ToString().Substring(0, 3); 
            await _userRepository.UpdateDailyStreak(_userId, today, true);

            // Update the UI for daily streaks
            var dailyStreaks = await _userRepository.GetDailyStreaks(_userId);
            DashboardViewModel.UpdateDailyStreaks(dailyStreaks);
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
