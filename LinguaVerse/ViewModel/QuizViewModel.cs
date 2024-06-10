using LinguaVerse.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace LinguaVerse.ViewModel
{
    public class QuizViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Question> Questions { get; set; }
        private string _result = string.Empty;
        private int _points = 0;

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
                IsResultVisible = true;
            }
        }

        public int Points
        {
            get => _points;
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }

        private bool _isResultVisible = false;
        public bool IsResultVisible
        {
            get => _isResultVisible;
            set
            {
                _isResultVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand CheckAnswersCommand { get; }
        public ICommand NavigateCommand { get; }

        public QuizViewModel()
        {
            Questions = new ObservableCollection<Question>
            {
                new Question
        {
            QuestionText = "How do you say 'Hello' in Italian?",
            Answer = "Ciao",
            Choices = new ObservableCollection<string> { "Ciao", "Bonjour", "Hola", "Hallo" }
        },
        new Question
        {
            QuestionText = "What is the Italian word for 'Thank you'?",
            Answer = "Grazie",
            Choices = new ObservableCollection<string> { "Gracias", "Merci", "Danke", "Grazie" }
        },
        new Question
        {
            QuestionText = "How do you say 'Goodbye' in Italian?",
            Answer = "Arrivederci",
            Choices = new ObservableCollection<string> { "Adiós", "Auf Wiedersehen", "Arrivederci", "Au revoir" }
        },
        new Question
        {
            QuestionText = "What is the translation of 'Please' in Italian?",
            Answer = "Per favore",
            Choices = new ObservableCollection<string> { "Por favor", "S'il vous plaît", "Bitte", "Per favore" }
        },
        new Question
        {
            QuestionText = "How do you say 'Yes' in Italian?",
            Answer = "Sì",
            Choices = new ObservableCollection<string> { "Oui", "Sí", "Ja", "Sì" }
        },
            };

            CheckAnswersCommand = new Command(CheckAnswers);
            NavigateCommand = new Command(Navigate);
        }

        private void CheckAnswers()
        {
            int correctAnswers = Questions.Count(q => q.Answer == q.SelectedAnswer);
            Points += correctAnswers * 2; // 2 points for each correct answer
            Result = correctAnswers == Questions.Count ? "Correct" : "Wrong";
        }

        private async void Navigate()
        {
            // Pass the points to the next page
            var nextPage = new Views.SecondPage();
            ((SecondQuizViewModel)nextPage.BindingContext).Points = this.Points;
            await Application.Current.MainPage.Navigation.PushAsync(nextPage);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
