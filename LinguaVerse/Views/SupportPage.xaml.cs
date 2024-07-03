using System;
using Microsoft.Maui.Controls;
using LinguaVerse.ViewModel;

namespace LinguaVerse
{
    public partial class SupportPage : ContentPage
    {
        public SupportPage()
        {
            InitializeComponent();
            BindingContext = new SupportViewModel();
        }
    }
}
