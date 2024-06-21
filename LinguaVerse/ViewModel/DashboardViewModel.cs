using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using LinguaVerse.DAL;
using LinguaVerse.Model;
using LinguaVerse.Views;
using Microsoft.Maui.Controls;
using static LinguaVerse.DAL.UserRepository;

namespace LinguaVerse.ViewModel
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        private int _userId;
        public ICommand NavigateToQuizHistoryCommand { get; }

        public DashboardViewModel(UserRepository userRepository, int userId)
        {
            _userRepository = userRepository;
            _userId = userId;
            NavigateToLanguageSelectionCommand = new Command(NavigateToLanguageSelection);
            NavigateToQuizHistoryCommand = new Command(NavigateToQuizHistory);
            NavigateToTestPage1Command = new Command(NavigateToTestPage1); 
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
                _progress = value;
                OnPropertyChanged();
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

        public ICommand NavigateToLanguageSelectionCommand { get; }

        public async void LoadUserData()
        {
            System.Diagnostics.Debug.WriteLine($"Loading data for user ID: {_userId}");

            var user = await _userRepository.GetUserById(_userId);
            if (user == null)
            {
                System.Diagnostics.Debug.WriteLine($"User with ID {_userId} not found.");
                await Application.Current.MainPage.DisplayAlert("Error", "User not found", "OK");
                return;
            }

            Username = user.Username;
            WelcomeMessage = $"Welcome back, {user.Username}";

            var dailyStreaks = await _userRepository.GetDailyStreaks(_userId);
            if (!dailyStreaks.Any())
            {
                // Initialize default daily streaks
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

            var courseProgress = (await _userRepository.GetCourseProgress(user.UserID))
                .Select(cp => new CourseProgress
                {
                    CourseName = cp.CourseName,
                    Progress = cp.Progress,
                    Level = cp.Level
                });
            CourseProgress = new ObservableCollection<CourseProgress>(courseProgress);

            var featuredCourses = (await _userRepository.GetFeaturedCourses())
                .Select(fc => new FeaturedCourse
                {
                    CourseName = fc.CourseName,
                    Duration = fc.Duration,
                    Questions = fc.Questions,
                    Level = fc.Level,
                    FlagIcon = fc.FlagIcon
                });
            FeaturedCourses = new ObservableCollection<FeaturedCourse>(featuredCourses);

            var userProgresses = (await _userRepository.GetUserProgressAsync(user.UserID))
                .Select(up => new UserProgress
                {
                    UserProgressID = up.UserProgressID,
                    UserID = up.UserID,
                    QuizID = up.QuizID,
                    Score = up.Score,
                    CompletionTime = up.CompletionTime,
                    AttemptDate = up.AttemptDate
                });
            UserProgresses = new ObservableCollection<UserProgress>(userProgresses);
        }

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

        public void UpdateDailyStreaks(IEnumerable<DailyStreak> dailyStreaks)
        {
            // Order the days correctly
            dailyStreaks = dailyStreaks.OrderBy(ds => GetDayOrder(ds.Day)).ToList();
            DailyStreaks = new ObservableCollection<DailyStreak>(dailyStreaks);
        }

        private async void NavigateToLanguageSelection()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.LanguageSelection());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void NavigateToQuizHistory()
        {
            var quizHistoryViewModel = App.Services.GetRequiredService<Func<int, QuizHistoryViewModel>>()(App.CurrentUserId);
            var quizHistoryPage = new QuizHistoryPage(quizHistoryViewModel);
            await Application.Current.MainPage.Navigation.PushAsync(quizHistoryPage);
        }

        public ICommand NavigateToTestPage1Command { get; }
        private async void NavigateToTestPage1()
        {
            var testPage = new TestPage1(); 
            await Application.Current.MainPage.Navigation.PushAsync(testPage);
        }

    }
}
