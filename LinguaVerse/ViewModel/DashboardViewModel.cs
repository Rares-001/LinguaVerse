using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Threading.Tasks;
using LinguaVerse.DAL;
using Microsoft.Maui.Controls;

namespace LinguaVerse.ViewModel
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;

        public DashboardViewModel(UserRepository userRepository, string username)
        {
            _userRepository = userRepository;
            Username = username;
            LoadUserData(username);

            NavigateToLanguageSelectionCommand = new Command(NavigateToLanguageSelection);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        private string _welcomeMessage;
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

        private ObservableCollection<string> _dailyStreaks;
        public ObservableCollection<string> DailyStreaks
        {
            get => _dailyStreaks;
            set
            {
                _dailyStreaks = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CourseProgress> _courseProgress;
        public ObservableCollection<CourseProgress> CourseProgress
        {
            get => _courseProgress;
            set
            {
                _courseProgress = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<FeaturedCourse> _featuredCourses;
        public ObservableCollection<FeaturedCourse> FeaturedCourses
        {
            get => _featuredCourses;
            set
            {
                _featuredCourses = value;
                OnPropertyChanged();
            }
        }

        public ICommand NavigateToLanguageSelectionCommand { get; }

        private async void LoadUserData(string username)
        {
            var user = await _userRepository.GetUserByUsername(username);
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
    }

    public class CourseProgress
    {
        public string CourseName { get; set; }
        public float Progress { get; set; }
        public string Level { get; set; }
    }

    public class FeaturedCourse
    {
        public string CourseName { get; set; }
        public int Duration { get; set; }
        public int Questions { get; set; }
        public string Level { get; set; }
        public string FlagIcon { get; set; }
    }
}
