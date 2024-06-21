using System;
using System.Collections.Generic;
using LinguaVerse.Model;

namespace LinguaVerse.ViewModel
{
    public class MemoryCardViewModel
    {
        // List of card sets containing memory cards
        public List<List<MemoryCard>> CardSets { get; }

        // Constructor initializes the card sets and shuffles them
        public MemoryCardViewModel()
        {
            CardSets = new List<List<MemoryCard>>
            {
                new List<MemoryCard>
                {
                    new MemoryCard("Question 1", "Answer 1"),
                    new MemoryCard("Answer 1", "Question 1"),
                    new MemoryCard("Question 2", "Answer 2"),
                    new MemoryCard("Answer 2", "Question 2"),
                    new MemoryCard("Question 3", "Answer 3"),
                    new MemoryCard("Answer 3", "Question 3")
                },
                new List<MemoryCard>
                {
                    new MemoryCard("Question 4", "Answer 4"),
                    new MemoryCard("Answer 4", "Question 4"),
                    new MemoryCard("Question 5", "Answer 5"),
                    new MemoryCard("Answer 5", "Question 5"),
                    new MemoryCard("Question 6", "Answer 6"),
                    new MemoryCard("Answer 6", "Question 6")
                },
                new List<MemoryCard>
                {
                    new MemoryCard("Question 7", "Answer 7"),
                    new MemoryCard("Answer 7", "Question 7"),
                    new MemoryCard("Question 8", "Answer 8"),
                    new MemoryCard("Answer 8", "Question 8"),
                    new MemoryCard("Question 9", "Answer 9"),
                    new MemoryCard("Answer 9", "Question 9")
                },
                new List<MemoryCard>
                {
                    new MemoryCard("Question 10", "Answer 10"),
                    new MemoryCard("Answer 10", "Question 10"),
                    new MemoryCard("Question 11", "Answer 11"),
                    new MemoryCard("Answer 11", "Question 11"),
                    new MemoryCard("Question 12", "Answer 12"),
                    new MemoryCard("Answer 12", "Question 12")
                },
                new List<MemoryCard>
                {
                    new MemoryCard("Question 13", "Answer 13"),
                    new MemoryCard("Answer 13", "Question 13"),
                    new MemoryCard("Question 14", "Answer 14"),
                    new MemoryCard("Answer 14", "Question 14"),
                    new MemoryCard("Question 15", "Answer 15"),
                    new MemoryCard("Answer 15", "Question 15")
                }
            };

            // Shuffle each card set
            foreach (var cardSet in CardSets)
            {
                cardSet.Shuffle();
            }
        }
    }

    // Extension method to shuffle a list
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

            // Ensure no two consecutive cards are the same
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
