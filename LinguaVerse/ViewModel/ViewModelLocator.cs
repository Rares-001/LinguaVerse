using LinguaVerse.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace LinguaVerse.ViewModel
{
    public class ViewModelLocator
    {
        public DashboardViewModel DashboardViewModel => App.Services.GetRequiredService<Func<int, DashboardViewModel>>()(App.CurrentUserId);
        public QuizViewModel QuizViewModel => App.Services.GetRequiredService<Func<UserRepository, DashboardViewModel, int, QuizViewModel>>()(
            App.Services.GetRequiredService<UserRepository>(),
            DashboardViewModel,
            App.CurrentUserId
        );
        public LoginViewModel LoginViewModel => App.Services.GetRequiredService<LoginViewModel>();
        public LanguageSelectionViewModel LanguageSelectionViewModel => App.Services.GetRequiredService<LanguageSelectionViewModel>();
        public QuizHistoryViewModel QuizHistoryViewModel => App.Services.GetRequiredService<Func<int, QuizHistoryViewModel>>()(App.CurrentUserId);
    }
}
