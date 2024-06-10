namespace LinguaVerse.Model
{
    public class UserProgress
    {
        public int UserProgressID { get; set; }
        public int UserID { get; set; }
        public int QuizID { get; set; }
        public int Score { get; set; }
        public int CompletionTime { get; set; }
        public DateTime AttemptDate { get; set; }
    }
}
