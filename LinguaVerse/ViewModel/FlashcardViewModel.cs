using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace LinguaVerse.ViewModels
{
    public class FlashcardViewModel : INotifyPropertyChanged
    {
        private int _currentIndex;

        public ObservableCollection<Flashcard> Flashcards { get; set; }
        public Flashcard CurrentFlashcard => Flashcards[_currentIndex];

        public ICommand ShowAnswerCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand FlipCommand { get; }

        public FlashcardViewModel()
        {
            Flashcards = new ObservableCollection<Flashcard>
            {
                new Flashcard { Question = "What is C#?", Answer = "A programming language developed by Microsoft." },
                new Flashcard { Question = "What are the key features of C#?", Answer = "Some key features include:Object-oriented," +
                "Type-safe," +
                "Scalable and updateable," +
                "Component-oriented,Rich standard library" },
                new Flashcard { Question = "What is the difference between == and Equals() in C#?", Answer = "The == operator checks for reference equality for reference " +
                "types and value equality for value types." +
                " The Equals() method checks for value equality " +
                "and can be overridden in custom classes to provide specific equality logic." },
                new Flashcard { Question = "What is the purpose of the using statement in C#?", Answer = "The using statement ensures that resources are disposed of correctly. " +
                "It is typically used for handling IDisposable objects like file streams, database connections, etc.," +
                "ensuring they are properly closed and disposed of when they are no longer needed." },
                new Flashcard { Question = "What is the purpose of the using statement in C#?", Answer = "The using statement ensures that resources are disposed of correctly." +
                " It is typically used for handling IDisposable objects like file streams, database connections, etc., ensuring they are properly closed and disposed" +
                " of when they are no longer needed." },
                new Flashcard { Question = "What is inheritance in C#?", Answer = " Inheritance is a fundamental concept of object-oriented programming" +
                " that allows a class to inherit properties and methods from another class." },
                new Flashcard { Question = "What is an interface in C#?", Answer = "An interface is a contract that defines a set of methods " +
                "and properties that a class must implement." +
                " Interfaces allow for defining functionalities without implementing them," +
                " which can be implemented by any class or struct." },
                new Flashcard { Question = "What are delegates in C#?", Answer = "Delegates are type-safe pointers to methods." +
                " They can be used to pass methods as arguments to other methods, allowing for dynamic method invocation. " },
                new Flashcard { Question = "What is XAML?", Answer = "A markup language for designing UI in .NET applications." },
                new Flashcard { Question = "What is .NET MAUI?", Answer = "A framework for building cross-platform apps." },
                new Flashcard { Question = "What is C#?", Answer = "A programming language developed by Microsoft." },
            };

            ShowAnswerCommand = new Command(ShowAnswer);
            PreviousCommand = new Command(PreviousFlashcard);
            NextCommand = new Command(NextFlashcard);
            FlipCommand = new Command(Flip);
            _currentIndex = 0;
        }

        private void ShowAnswer()
        {
        
        }

        private void PreviousFlashcard()
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                OnPropertyChanged(nameof(CurrentFlashcard));
            }
        }

        private void NextFlashcard()
        {
            if (_currentIndex < Flashcards.Count - 1)
            {
                _currentIndex++;
                OnPropertyChanged(nameof(CurrentFlashcard));
            }
        }

        private void Flip()
        {
            // This is handled in the MainPage.xaml.cs 
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
