using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LinguaVerse.Model;
using Microsoft.Maui.Controls;

// PAGE TO BE REMOVED FROM THE PROJECT
namespace LinguaVerse.ViewModel
{
    public class SecondQuizViewModel : INotifyPropertyChanged
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

        public ICommand CheckAnswersCommand { get; }

        public SecondQuizViewModel()
        {
            Questions = new ObservableCollection<Question>
            {
                new Question
                {
                    QuestionText = "What is the Italian word for 'Book'?",
                    Answer = "Libro",
                    Choices = new ObservableCollection<string> { "Libro", "Livre", "Buch", "Libro" }
                },
                new Question
                {
                    QuestionText = "How do you say 'House' in Italian?",
                    Answer = "Casa",
                    Choices = new ObservableCollection<string> { "Casa", "Maison", "Haus", "Casa" }
                },
                new Question
                {
                    QuestionText = "What does 'Famiglia' mean in English?",
                    Answer = "Family",
                    Choices = new ObservableCollection<string> { "Family", "Familia", "Famille", "Familie" }
                }
            };

            CheckAnswersCommand = new Command(CheckAnswers);
        }

        private void CheckAnswers()
        {
            int correctAnswers = Questions.Count(q => q.Answer == q.SelectedAnswer);
            Points += correctAnswers * 2; // 2 points for each correct answer, still missing
            Result = correctAnswers == Questions.Count ? "Correct" : "Wrong";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
