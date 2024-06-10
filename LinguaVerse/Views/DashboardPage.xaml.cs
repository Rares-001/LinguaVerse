using Microsoft.Maui.Controls;
using LinguaVerse.ViewModel;

namespace LinguaVerse.Views
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage()
        {
            InitializeComponent();
        }

        public DashboardPage(DashboardViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
