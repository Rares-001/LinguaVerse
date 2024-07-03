using System;
using System.Collections.Generic;
using LinguaVerse.Model;

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
                    new Model.MemoryCard("What is a synonym for 'happy'?", "Joyful"),
                    new Model.MemoryCard("Joyful", "What is a synonym for 'happy'?"),
                    new Model.MemoryCard("What is the past tense of 'run'?", "Ran"),
                    new Model.MemoryCard("Ran", "What is the past tense of 'run'?"),
                    new Model.MemoryCard("What is an antonym for 'hot'?", "Cold"),
                    new Model.MemoryCard("Cold", "What is an antonym for 'hot'?")
                },
                new List<Model.MemoryCard>
                {
                    new Model.MemoryCard("What is the plural of 'child'?", "Children"),
                    new Model.MemoryCard("Children", "What is the plural of 'child'?"),
                    new Model.MemoryCard("What is a synonym for 'big'?", "Large"),
                    new Model.MemoryCard("Large", "What is a synonym for 'big'?"),
                    new Model.MemoryCard("What is the past tense of 'eat'?", "Ate"),
                    new Model.MemoryCard("Ate", "What is the past tense of 'eat'?")
                },
                new List<Model.MemoryCard>
                {
                    new Model.MemoryCard("What is an antonym for 'fast'?", "Slow"),
                    new Model.MemoryCard("Slow", "What is an antonym for 'fast'?"),
                    new Model.MemoryCard("What is the plural of 'mouse'?", "Mice"),
                    new Model.MemoryCard("Mice", "What is the plural of 'mouse'?"),
                    new Model.MemoryCard("What is a synonym for 'small'?", "Tiny"),
                    new Model.MemoryCard("Tiny", "What is a synonym for 'small'?")
                },
                new List<Model.MemoryCard>
                {
                    new Model.MemoryCard("What is the past tense of 'go'?", "Went"),
                    new Model.MemoryCard("Went", "What is the past tense of 'go'?"),
                    new Model.MemoryCard("What is an antonym for 'up'?", "Down"),
                    new Model.MemoryCard("Down", "What is an antonym for 'up'?"),
                    new Model.MemoryCard("What is the plural of 'foot'?", "Feet"),
                    new Model.MemoryCard("Feet", "What is the plural of 'foot'?")
                },
                new List<Model.MemoryCard>
                {
                    new Model.MemoryCard("What is the past tense of 'write'?", "Wrote"),
                    new Model.MemoryCard("Wrote", "What is the past tense of 'write'?"),
                    new Model.MemoryCard("What is a synonym for 'quick'?", "Rapid"),
                    new Model.MemoryCard("Rapid", "What is a synonym for 'quick'?"),
                    new Model.MemoryCard("What is an antonym for 'young'?", "Old"),
                    new Model.MemoryCard("Old", "What is an antonym for 'young'?")
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

            // Ensure that no pairs are placed next to each other
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