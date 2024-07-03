using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LinguaVerse.Model;
using Microsoft.Maui.Controls;

namespace LinguaVerse.ViewModel
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
                new Flashcard { Question = "What is a noun?", Answer = "A noun is a word that represents a person, place, thing, or idea.", ImagePath = "image1.jpg" },
                new Flashcard { Question = "What is a verb?", Answer = "A verb is a word that represents an action, occurrence, or state of being.", ImagePath = "image2.jpg" },
                new Flashcard { Question = "What is an adjective?", Answer = "An adjective is a word that describes or modifies a noun.", ImagePath = "image3.jpg" },
                new Flashcard { Question = "What is an adverb?", Answer = "An adverb is a word that modifies a verb, adjective, or other adverb.", ImagePath = "image4.jpg" },
                new Flashcard { Question = "What is a pronoun?", Answer = "A pronoun is a word that takes the place of a noun.", ImagePath = "image5.jpg" },
                new Flashcard { Question = "What is a preposition?", Answer = "A preposition is a word that shows the relationship between a noun (or pronoun) and other words in a sentence.", ImagePath = "image6.jpg" },
                new Flashcard { Question = "What is a conjunction?", Answer = "A conjunction is a word that connects clauses or sentences.", ImagePath = "image7.jpg" },
                new Flashcard { Question = "What is an interjection?", Answer = "An interjection is a word or phrase that expresses emotion or exclamation.", ImagePath = "image8.jpg" },
                new Flashcard { Question = "What is a sentence?", Answer = "A sentence is a group of words that expresses a complete thought.", ImagePath = "image9.jpg" },
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
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
