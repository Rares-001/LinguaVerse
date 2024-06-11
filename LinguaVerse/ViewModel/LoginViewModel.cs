using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LinguaVerse.DAL;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using LinguaVerse.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LinguaVerse.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly UserRepository _userRepository;
        private readonly IServiceProvider _serviceProvider;

        public LoginViewModel(UserRepository userRepository, IServiceProvider serviceProvider)
        {
            _userRepository = userRepository;
            _serviceProvider = serviceProvider;
            CheckDatabaseConnection();
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

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _connectionStatus = string.Empty;
        public string ConnectionStatus
        {
            get => _connectionStatus;
            private set
            {
                _connectionStatus = value;
                OnPropertyChanged();
            }
        }

        private string _registrationStatus = string.Empty;
        public string RegistrationStatus
        {
            get => _registrationStatus;
            private set
            {
                _registrationStatus = value;
                OnPropertyChanged();
            }
        }

        private string _loginStatus = string.Empty;
        public string LoginStatus
        {
            get => _loginStatus;
            private set
            {
                _loginStatus = value;
                OnPropertyChanged();
            }
        }

        public ICommand RegisterCommand => new Command(async () => await RegisterUser());
        public ICommand LoginCommand => new Command(async () => await LoginUser());

        private async Task RegisterUser()
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
        }

        private async Task LoginUser()
        {
            var isSuccess = await _userRepository.LoginUser(Username, Password);
            if (isSuccess)
            {
                LoginStatus = "Login Successful";
                await Application.Current.MainPage.DisplayAlert("Success", "Login Successful", "OK");

                // Get the user ID
                var user = await _userRepository.GetUserByUsername(Username);
                if (user == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "User not found after login", "OK");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Logged in user ID: {user.UserID}");

                // Navigate to DashboardPage
                var dashboardPage = _serviceProvider.GetRequiredService<DashboardPage>();
                var dashboardViewModel = dashboardPage.BindingContext as DashboardViewModel;
                dashboardViewModel?.Initialize(user.UserID);
                await Application.Current.MainPage.Navigation.PushAsync(dashboardPage);
            }
            else
            {
                LoginStatus = "Login Failed";
                await Application.Current.MainPage.DisplayAlert("Error", "Login Failed", "OK");
            }
        }

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
