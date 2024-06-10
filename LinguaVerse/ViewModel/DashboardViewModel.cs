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
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        private readonly int _userId;

        public DashboardViewModel(UserRepository userRepository, int userId)
        {
            _userRepository = userRepository;
            _userId = userId;

            LoadUserData();
            NavigateToLanguageSelectionCommand = new Command(NavigateToLanguageSelection);
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

        private ObservableCollection<string> _dailyStreaks = new ObservableCollection<string>();
        public ObservableCollection<string> DailyStreaks
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

        private async void LoadUserData()
        {
            var user = await _userRepository.GetUserById(_userId);
            Username = user.Username;
            WelcomeMessage = $"Welcome back, {user.Username}";

            var dailyStreaks = await _userRepository.GetDailyStreaks(user.UserID);
            DailyStreaks = new ObservableCollection<string>(dailyStreaks);

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

        private async void NavigateToLanguageSelection()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new Views.LanguageSelection());
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CourseProgress
    {
        public string CourseName { get; set; } = string.Empty;
        public float Progress { get; set; }
        public string Level { get; set; } = string.Empty;
    }

    public class FeaturedCourse
    {
        public string CourseName { get; set; } = string.Empty;
        public int Duration { get; set; }
        public int Questions { get; set; }
        public string Level { get; set; } = string.Empty;
        public string FlagIcon { get; set; } = string.Empty;
    }

    public class UserProgress
    {
        public int UserProgressID { get; set; }
        public int UserID { get; set; }
        public int QuizID { get; set; }
        public int Score { get; set; }
        public int CompletionTime { get; set; }
        public DateTime AttemptDate { get; set; }
    }
}
