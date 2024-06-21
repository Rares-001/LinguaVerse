using System.Collections.ObjectModel;

public class TestQuestion
{
    public int QuestionID { get; set; }
    public string QuestionText { get; set; }
    public string Answer { get; set; }
    public ObservableCollection<string> Choices { get; set; }
    public string SelectedAnswer { get; set; }
    public bool IsCorrect { get; set; }

}
