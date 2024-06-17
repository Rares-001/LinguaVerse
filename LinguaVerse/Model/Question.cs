using System.Collections.ObjectModel;

public class Question
{
    public int QuestionID { get; set; }
    public int QuizID { get; set; }
    public string QuestionText { get; set; }
    public string Answer { get; set; }
    public ObservableCollection<string> Choices { get; set; }
    public string Explanation { get; set; }
    public string SelectedAnswer { get; set; }
    public bool? IsCorrect { get; set; } 
    public bool ShowExplanation { get; set; } 
}
