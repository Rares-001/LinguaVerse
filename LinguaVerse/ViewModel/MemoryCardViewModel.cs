using System.Collections.Generic;
using System.Linq;

namespace LinguaVerse.ViewModel
{
    public class MemoryCardViewModel
    {
        public List<Model.MemoryCard> Cards { get; }

        public MemoryCardViewModel()
        {
            Cards = new List<Model.MemoryCard>
            {
                new Model.MemoryCard("Question 1", "Answer 1"),
                new Model.MemoryCard("Answer 1", "Question 1"),
                new Model.MemoryCard("Question 2", "Answer 2"),
                new Model.MemoryCard("Answer 2", "Question 2")
            };

            Cards = Cards.OrderBy(c => System.Guid.NewGuid()).ToList(); 
        }
    }
}
