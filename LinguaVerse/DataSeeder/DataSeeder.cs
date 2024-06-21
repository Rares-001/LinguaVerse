using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataSeeder
{
    class Program
    {
        private static string _connectionString = "Host=localhost;Database=LinguaVerseDB;Username=postgres;Password=admin";

        static async Task Main(string[] args)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var questions = new List<Question>
            {
                new Question { QuizID = 1, QuestionText = "How do you say 'Good Morning' in Italian?", Answer = "Buongiorno", Choices = new[] { "Buongiorno", "Buona notte", "Buonasera", "Ciao" } },
                new Question { QuizID = 1, QuestionText = "What is the Italian word for 'Good Night'?", Answer = "Buona notte", Choices = new[] { "Buongiorno", "Buona notte", "Buonasera", "Ciao" } },
                new Question { QuizID = 1, QuestionText = "How do you say 'See you later' in Italian?", Answer = "A dopo", Choices = new[] { "A dopo", "Arrivederci", "Ciao", "Addio" } },
                new Question { QuizID = 2, QuestionText = "How do you say 'Where is the bathroom?' in Italian?", Answer = "Dov'è il bagno?", Choices = new[] { "Dov'è il bagno?", "Dov'è la cucina?", "Dov'è la camera?", "Dov'è il salotto?" } },
                new Question { QuizID = 2, QuestionText = "What is the Italian word for 'Thank you very much'?", Answer = "Grazie mille", Choices = new[] { "Grazie mille", "Molto bene", "Grazie tanto", "Molto grazie" } },
                new Question { QuizID = 2, QuestionText = "How do you say 'Excuse me' in Italian?", Answer = "Mi scusi", Choices = new[] { "Mi scusi", "Per favore", "Grazie", "Scusa" } },
                new Question { QuizID = 3, QuestionText = "What is the translation of 'Please' in Italian?", Answer = "Per favore", Choices = new[] { "Per favore", "Grazie", "Scusa", "Prego" } },
                new Question { QuizID = 3, QuestionText = "How do you say 'Yes' in Italian?", Answer = "Sì", Choices = new[] { "Sì", "No", "Forse", "Mai" } },
                new Question { QuizID = 3, QuestionText = "Which of the following is a form of the verb 'essere'?", Answer = "Sono", Choices = new[] { "Sono", "Sei", "Siamo", "Siete" } },
                new Question { QuizID = 4, QuestionText = "How do you say 'I would like a coffee' in Italian?", Answer = "Vorrei un caffè", Choices = new[] { "Vorrei un caffè", "Voglio un caffè", "Prendo un caffè", "Dò un caffè" } },
                new Question { QuizID = 4, QuestionText = "What is the Italian word for 'book'?", Answer = "Libro", Choices = new[] { "Libro", "Libretto", "Libreria", "Libraio" } },
                new Question { QuizID = 4, QuestionText = "How do you say 'I am lost' in Italian?", Answer = "Mi sono perso", Choices = new[] { "Mi sono perso", "Sono perso", "Mi perdo", "Perduto" } },
                // Add more questions here as needed
            };

            const string query = @"
                INSERT INTO ""Questions"" (""QuizID"", ""QuestionText"", ""Answer"", ""Choices"")
                VALUES (@QuizID, @QuestionText, @Answer, @Choices)";

            foreach (var question in questions)
            {
                await connection.ExecuteAsync(query, question);
            }

            Console.WriteLine("Questions have been seeded successfully.");
        }

        public class Question
        {
            public int QuizID { get; set; }
            public string QuestionText { get; set; }
            public string Answer { get; set; }
            public string[] Choices { get; set; }
        }
    }
}
