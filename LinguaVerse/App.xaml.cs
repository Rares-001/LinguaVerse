using LinguaVerse.ViewModel;
using LinguaVerse.Views;
using Microsoft.Maui.Controls;
using System;

namespace LinguaVerse
{
    public partial class App : Application
    {
        public static IServiceProvider Services { get; internal set; }
        public static int CurrentUserId { get; set; }  // Store the current user ID

        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            Services = serviceProvider;

            // Set the initial MainPage to the LoginPage
            MainPage = new NavigationPage(new LoginPage(Services.GetService<LoginViewModel>()));
        }
    }
}
