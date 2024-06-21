using System;
using System.Collections.Generic;
using System.Linq;

namespace LinguaVerse.ViewModel
{
    public class MemoryCardViewModel
    {
        public List<List<Model.MemoryCard>> CardSets { get; }

        public MemoryCardViewModel()
        {
            CardSets = new List<List<Model.MemoryCard>>
            {
                new List<Model.MemoryCard>
                {
                    new Model.MemoryCard("Question 1", "Answer 1"),
                    new Model.MemoryCard("Answer 1", "Question 1"),
                    new Model.MemoryCard("Question 2", "Answer 2"),
                    new Model.MemoryCard("Answer 2", "Question 2"),
                    new Model.MemoryCard("Question 3", "Answer 3"),
                    new Model.MemoryCard("Answer 3", "Question 3")
                },
                new List<Model.MemoryCard>
                {
                    new Model.MemoryCard("Question 4", "Answer 4"),
                    new Model.MemoryCard("Answer 4", "Question 4"),
                    new Model.MemoryCard("Question 5", "Answer 5"),
                    new Model.MemoryCard("Answer 5", "Question 5"),
                    new Model.MemoryCard("Question 6", "Answer 6"),
                    new Model.MemoryCard("Answer 6", "Question 6")
                },
                new List<Model.MemoryCard>
                {
                    new Model.MemoryCard("Question 7", "Answer 7"),
                    new Model.MemoryCard("Answer 7", "Question 7"),
                    new Model.MemoryCard("Question 8", "Answer 8"),
                    new Model.MemoryCard("Answer 8", "Question 8"),
                    new Model.MemoryCard("Question 9", "Answer 9"),
                    new Model.MemoryCard("Answer 9", "Question 9")
                },
                new List<Model.MemoryCard>
                {
                    new Model.MemoryCard("Question 10", "Answer 10"),
                    new Model.MemoryCard("Answer 10", "Question 10"),
                    new Model.MemoryCard("Question 11", "Answer 11"),
                    new Model.MemoryCard("Answer 11", "Question 11"),
                    new Model.MemoryCard("Question 12", "Answer 12"),
                    new Model.MemoryCard("Answer 12", "Question 12")
                },
                new List<Model.MemoryCard>
                {
                    new Model.MemoryCard("Question 13", "Answer 13"),
                    new Model.MemoryCard("Answer 13", "Question 13"),
                    new Model.MemoryCard("Question 14", "Answer 14"),
                    new Model.MemoryCard("Answer 14", "Question 14"),
                    new Model.MemoryCard("Question 15", "Answer 15"),
                    new Model.MemoryCard("Answer 15", "Question 15")
                }
            };

            foreach (var cardSet in CardSets)
            {
                cardSet.Shuffle();
            }
        }
    }

    public static class ListExtensions
    {
        private static readonly Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            for (int i = 0; i < list.Count - 1; i += 2)
            {
                if (list[i].Equals(list[i + 1]))
                {
                    int swapIndex = (i + 2 < list.Count) ? i + 2 : 0;
                    T temp = list[i + 1];
                    list[i + 1] = list[swapIndex];
                    list[swapIndex] = temp;
                }
            }
        }
    }
}
