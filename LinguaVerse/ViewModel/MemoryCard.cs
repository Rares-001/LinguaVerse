namespace LinguaVerse.Model
{
    public class MemoryCard
    {
        public string Text { get; }
        public string Match { get; }

        public MemoryCard(string text, string match)
        {
            Text = text;
            Match = match;
        }
    }
}
