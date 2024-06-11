using System.Collections.ObjectModel;

namespace LinguaVerse.Model
{
    public class Question
    {
        public int QuestionID { get; set; }
        public int QuizID { get; set; }
        public string QuestionText { get; set; }
        public string Answer { get; set; }
        public string? SelectedAnswer { get; set; }
        public ObservableCollection<string> Choices { get; set; }
    }
}
