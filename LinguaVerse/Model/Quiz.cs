namespace LinguaVerse.Model
{
    public class Quiz
    {
        public int QuizID { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string Level { get; set; }
        public string? SelectedAnswer { get; set; }
    }
}
