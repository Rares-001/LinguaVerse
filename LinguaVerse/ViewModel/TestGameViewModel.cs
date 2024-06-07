using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LinguaVerse.Model;
using Microsoft.Maui.Controls;

namespace LinguaVerse.ViewModels
{
    public class TestGameViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<TestQuestion> Questions { get; set; }
        private string _result = string.Empty; 

        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged();
            }
        }

        public ICommand CheckAnswersCommand { get; }

        public TestGameViewModel()
        {
            Questions = new ObservableCollection<TestQuestion>
            {
                new TestQuestion
                {
                    QuestionText = "What is the capital of France?",
                    Choices = new ObservableCollection<string> { "Paris", "London", "Rome", "Berlin" },
                    CorrectAnswer = "Paris"
                },
                new TestQuestion
                {
                    QuestionText = "What is 2 + 2?",
                    Choices = new ObservableCollection<string> { "3", "4", "5", "6" },
                    CorrectAnswer = "4"
                },
            };

            CheckAnswersCommand = new Command(CheckAnswers);
        }

        private void CheckAnswers()
        {
            int correctAnswers = Questions.Count(q => q.CorrectAnswer == q.SelectedAnswer);
            Result = $"You got {correctAnswers} out of {Questions.Count} correct!";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
