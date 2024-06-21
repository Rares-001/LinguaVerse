using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using LinguaVerse.DAL;
using LinguaVerse.Model;
using Microsoft.Maui.Controls;

namespace LinguaVerse.ViewModel
{
    public class QuizHistoryViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        private readonly int _userId;

        // Constructor initializes the repository and user ID, and sets up the LoadQuizHistoryCommand
        public QuizHistoryViewModel(UserRepository userRepository, int userId)
        {
            _userRepository = userRepository;
            _userId = userId;
            LoadQuizHistoryCommand = new Command(async () => await LoadQuizHistory());
            LoadQuizHistory();
        }

        private ObservableCollection<UserProgress> _quizHistory = new ObservableCollection<UserProgress>();
        public ObservableCollection<UserProgress> QuizHistory
        {
            get => _quizHistory;
            set
            {
                _quizHistory = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadQuizHistoryCommand { get; }

        // Loads the quiz history for the user
        private async Task LoadQuizHistory()
        {
            var history = await _userRepository.GetUserProgressAsync(_userId);
            QuizHistory = new ObservableCollection<UserProgress>(history);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
