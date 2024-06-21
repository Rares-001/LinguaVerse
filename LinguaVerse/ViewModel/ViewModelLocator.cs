using LinguaVerse.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace LinguaVerse.ViewModel
{
    public class ViewModelLocator
    {
        // Provides the DashboardViewModel instance with the current user ID
        public DashboardViewModel DashboardViewModel => App.Services.GetRequiredService<Func<int, DashboardViewModel>>()(App.CurrentUserId);

        // Provides the QuizViewModel instance with required dependencies
        public QuizViewModel QuizViewModel => App.Services.GetRequiredService<Func<UserRepository, DashboardViewModel, int, QuizViewModel>>()(
            App.Services.GetRequiredService<UserRepository>(),
            DashboardViewModel,
            App.CurrentUserId
        );

        // Provides the LoginViewModel instance
        public LoginViewModel LoginViewModel => App.Services.GetRequiredService<LoginViewModel>();

        // Provides the LanguageSelectionViewModel instance
        public LanguageSelectionViewModel LanguageSelectionViewModel => App.Services.GetRequiredService<LanguageSelectionViewModel>();

        // Provides the QuizHistoryViewModel instance with the current user ID
        public QuizHistoryViewModel QuizHistoryViewModel => App.Services.GetRequiredService<Func<int, QuizHistoryViewModel>>()(App.CurrentUserId);
    }
}
