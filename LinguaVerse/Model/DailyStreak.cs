namespace LinguaVerse.Model
{
    public class DailyStreak
    {
        public int StreakId { get; set; }
        public int UserId { get; set; }
        public string Day { get; set; }
        public bool IsCompleted { get; set; }
        public string Color { get; set; } = "White";
    }
}
