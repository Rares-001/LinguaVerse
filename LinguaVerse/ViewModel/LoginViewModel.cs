// ViewModel/LoginViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LinguaVerse.DAL;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace LinguaVerse.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;

        public LoginViewModel(UserRepository userRepository)
        {
            _userRepository = userRepository;
            CheckDatabaseConnection();
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

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _connectionStatus;
        public string ConnectionStatus
        {
            get => _connectionStatus;
            private set
            {
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        private string _registrationStatus;
        public string RegistrationStatus
        {
            get => _registrationStatus;
            private set
            {
                _registrationStatus = value;
                OnPropertyChanged();
            }
        }

        private string _loginStatus;
        public string LoginStatus
        {
            get => _loginStatus;
            private set
            {
                _loginStatus = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand => new Command(async () =>
        {
            var isSuccess = await _userRepository.RegisterUser(Username, Password);
            if (isSuccess)
            {
                RegistrationStatus = "Registration Successful";
                await Application.Current.MainPage.DisplayAlert("Success", "Registration Successful", "OK");
            }
            else
            {
                RegistrationStatus = "Registration Failed";
                await Application.Current.MainPage.DisplayAlert("Error", "Registration Failed", "OK");
            }
        });

        public ICommand LoginCommand => new Command(async () =>
        {
            var isSuccess = await _userRepository.LoginUser(Username, Password);
            if (isSuccess)
            {
                LoginStatus = "Login Successful";
                await Application.Current.MainPage.DisplayAlert("Success", "Login Successful", "OK");

                // Navigate to DashboardPage
                var dashboardViewModel = new DashboardViewModel(Username);
                await Application.Current.MainPage.Navigation.PushAsync(new Views.DashboardPage { BindingContext = dashboardViewModel });
            }
            else
            {
                LoginStatus = "Login Failed";
                await Application.Current.MainPage.DisplayAlert("Error", "Login Failed", "OK");
            }
        });

        private async void CheckDatabaseConnection()
        {
            var isConnected = await _userRepository.TestDatabaseConnection();
            ConnectionStatus = isConnected ? "Database Connected" : "Database Connection Failed";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
