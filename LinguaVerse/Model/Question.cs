using System.Collections.ObjectModel;

namespace LinguaVerse.Model
{
    public class Question
    {
        public string QuestionText { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public ObservableCollection<string> Choices { get; set; } = new ObservableCollection<string>();
        public string? SelectedAnswer { get; set; } 
    }
}