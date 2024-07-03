using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using LinguaVerse.DAL;
using LinguaVerse.Model;
using LinguaVerse.Views;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace LinguaVerse.ViewModel
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        private int _userId;
        private readonly ILogger<DashboardViewModel> _logger;
        public ICommand NavigateToSupportPageCommand { get; }

        public ICommand NavigateToQuizHistoryCommand { get; }
        public ICommand NavigateToLanguageSelectionCommand { get; }
        public ICommand NavigateToTestPage1Command { get; }

        // Constructor to initialize DashboardViewModel with necessary dependencies
        public DashboardViewModel(UserRepository userRepository, int userId, ILogger<DashboardViewModel> logger)
        {
            _userRepository = userRepository;
            _userId = userId;
            _logger = logger;

            NavigateToLanguageSelectionCommand = new Command(NavigateToLanguageSelection);
            NavigateToQuizHistoryCommand = new Command(NavigateToQuizHistory);
            NavigateToTestPage1Command = new Command(NavigateToTestPage1);
            NavigateToSupportPageCommand = new Command(NavigateToSupportPage);


            LoadUserData();
        }

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _welcomeMessage = string.Empty;
        public string WelcomeMessage
        {
            get => _welcomeMessage;
            set
            {
                _welcomeMessage = value;
                OnPropertyChanged();
            }
        }

        private float _progress;
        public float Progress
        {
            get => _progress;
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    _logger.LogInformation($"Progress value set: {_progress}");
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<DailyStreak> _dailyStreaks = new ObservableCollection<DailyStreak>();
        public ObservableCollection<DailyStreak> DailyStreaks
        {
            get => _dailyStreaks;
            set
            {
                _dailyStreaks = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CourseProgress> _courseProgress = new ObservableCollection<CourseProgress>();
        public ObservableCollection<CourseProgress> CourseProgress
        {
            get => _courseProgress;
            set
            {
                _courseProgress = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FeaturedCourse> _featuredCourses = new ObservableCollection<FeaturedCourse>();
        public ObservableCollection<FeaturedCourse> FeaturedCourses
        {
            get => _featuredCourses;
            set
            {
                _featuredCourses = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<UserProgress> _userProgresses = new ObservableCollection<UserProgress>();
        public ObservableCollection<UserProgress> UserProgresses
        {
            get => _userProgresses;
            set
            {
                _userProgresses = value;
                OnPropertyChanged();
            }
        }

        // Loads user data including username, welcome message, daily streaks, course progress, featured courses, and user progress
        public async void LoadUserData()
        {
            _logger.LogInformation($"Loading data for user ID: {_userId}");

            var user = await _userRepository.GetUserById(_userId);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {_userId} not found.");
                await Application.Current.MainPage.DisplayAlert("Error", "User not found", "OK");
                return;
            }

            Username = user.Username;
            WelcomeMessage = $"Welcome back, {user.Username}";

            var dailyStreaks = await _userRepository.GetDailyStreaks(_userId);
            if (!dailyStreaks.Any())
            {
                dailyStreaks = new List<DailyStreak>
                {
                    new DailyStreak { Day = "Mon", IsCompleted = false },
                    new DailyStreak { Day = "Tue", IsCompleted = false },
                    new DailyStreak { Day = "Wed", IsCompleted = false },
                    new DailyStreak { Day = "Thu", IsCompleted = false },
                    new DailyStreak { Day = "Fri", IsCompleted = false },
                    new DailyStreak { Day = "Sat", IsCompleted = false },
                    new DailyStreak { Day = "Sun", IsCompleted = false }
                };
            }

            dailyStreaks = dailyStreaks.OrderBy(ds => GetDayOrder(ds.Day)).ToList();
            DailyStreaks = new ObservableCollection<DailyStreak>(dailyStreaks.Select(ds => new DailyStreak
            {
                Day = ds.Day,
                IsCompleted = ds.IsCompleted
            }));

            var courseProgress = (await _userRepository.GetCourseProgress(user.UserID)).ToList();
            _logger.LogInformation($"Course Progress Count: {courseProgress.Count}");
            CourseProgress = new ObservableCollection<CourseProgress>(courseProgress.Select(cp => new CourseProgress
            {
                CourseName = cp.CourseName,
                Progress = cp.Progress,
                Level = cp.Level
            }));

            var featuredCourses = (await _userRepository.GetFeaturedCourses()).ToList();
            _logger.LogInformation($"Featured Courses Count: {featuredCourses.Count}");
            FeaturedCourses = new ObservableCollection<FeaturedCourse>(featuredCourses.Select(fc => new FeaturedCourse
            {
                CourseName = fc.CourseName,
                Duration = fc.Duration,
                Questions = fc.Questions,
                Level = fc.Level,
                FlagIcon = fc.FlagIcon
            }));

            var userProgresses = (await _userRepository.GetUserProgressAsync(user.UserID)).ToList();
            _logger.LogInformation($"User Progresses Count: {userProgresses.Count}");
            UserProgresses = new ObservableCollection<UserProgress>(userProgresses.Select(up => new UserProgress
            {
                UserProgressID = up.UserProgressID,
                UserID = up.UserID,
                QuizID = up.QuizID,
                Score = up.Score,
                CompletionTime = up.CompletionTime,
                AttemptDate = up.AttemptDate
            }));

            Progress = await _userRepository.CalculateUserProgressAsync(_userId);
            _logger.LogInformation($"Final Progress value set: {Progress}");
        }

        // Returns the order of the day in a week
        private int GetDayOrder(string day)
        {
            return day switch
            {
                "Mon" => 1,
                "Tue" => 2,
                "Wed" => 3,
                "Thu" => 4,
                "Fri" => 5,
                "Sat" => 6,
                "Sun" => 7,
                _ => 8,
            };
        }

        // Updates daily streaks with the provided list
        public void UpdateDailyStreaks(IEnumerable<DailyStreak> dailyStreaks)
        {
            dailyStreaks = dailyStreaks.OrderBy(ds => GetDayOrder(ds.Day)).ToList();
            DailyStreaks = new ObservableCollection<DailyStreak>(dailyStreaks);
        }

        // Navigates to the language selection page
        private async void NavigateToLanguageSelection()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.LanguageSelection());
        }

        // Navigates to the quiz history page
        private async void NavigateToQuizHistory()
        {
            var quizHistoryViewModel = App.Services.GetRequiredService<Func<int, QuizHistoryViewModel>>()(App.CurrentUserId);
            var quizHistoryPage = new QuizHistoryPage(quizHistoryViewModel);
            await Application.Current.MainPage.Navigation.PushAsync(quizHistoryPage);
        }

        // Navigates to the first test page
        private async void NavigateToTestPage1()
        {
            var testViewModel = new TestViewModel(_userRepository, this, _userId, 1, App.Services.GetRequiredService<ILogger<TestViewModel>>());
            var testPage = new TestPage1(testViewModel);
            await Application.Current.MainPage.Navigation.PushAsync(testPage);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void NavigateToSupportPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new SupportPage());
        }
    }
}
