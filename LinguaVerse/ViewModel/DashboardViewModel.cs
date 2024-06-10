// ViewModel/DashboardViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LinguaVerse.ViewModel
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
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

        // Additional properties for the dashboard
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DashboardViewModel(string username)
        {
            Username = username;
            // Initialize other properties as needed
            Progress = 0.5f; // Example progress value
        }
    }
}
