using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using LinguaVerse.Model;

namespace LinguaVerse.Seeders
{
    public class DataSeeder
    {
        private readonly IDbConnection _connection;

        public DataSeeder(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task SeedDataAsync()
        {
            // Insert quizzes
            var quizzes = new List<Quiz>
            {
                new Quiz { Title = "Basic Italian Phrases", Category = "Language", Level = "Beginner" },
                new Quiz { Title = "Advanced Italian Grammar", Category = "Language", Level = "Advanced" }
            };

            foreach (var quiz in quizzes)
            {
                await _connection.ExecuteAsync(
                    "INSERT INTO public.\"Quizzes\" (\"Title\", \"Category\", \"Level\") VALUES (@Title, @Category, @Level)",
                    quiz
                );
            }

            // Insert questions
            var questions = new List<Question>
            {
                new Question { QuizID = 1, QuestionText = "How do you say \"Hello\" in Italian?", Answer = "Ciao", Choices = new ObservableCollection<string> { "Ciao", "Bonjour", "Hola", "Hallo" } },
                new Question { QuizID = 1, QuestionText = "What is the Italian word for \"Thank you\"?", Answer = "Grazie", Choices = new ObservableCollection<string> { "Gracias", "Merci", "Danke", "Grazie" } },
                new Question { QuizID = 1, QuestionText = "How do you say \"Goodbye\" in Italian?", Answer = "Arrivederci", Choices = new ObservableCollection<string> { "Adiós", "Auf Wiedersehen", "Arrivederci", "Au revoir" } },
                new Question { QuizID = 1, QuestionText = "What is the translation of \"Please\" in Italian?", Answer = "Per favore", Choices = new ObservableCollection<string> { "Por favor", "S'il vous plaît", "Bitte", "Per favore" } },
                new Question { QuizID = 1, QuestionText = "How do you say \"Yes\" in Italian?", Answer = "Sì", Choices = new ObservableCollection<string> { "Oui", "Sí", "Ja", "Sì" } },

                new Question { QuizID = 2, QuestionText = "Which of the following is a subjunctive form of \"essere\" in Italian?", Answer = "sia", Choices = new ObservableCollection<string> { "sono", "sei", "sia", "siamo" } },
                new Question { QuizID = 2, QuestionText = "How do you say \"I would have eaten\" in Italian?", Answer = "Avrei mangiato", Choices = new ObservableCollection<string> { "Avrei mangiato", "Ho mangiato", "Mangerei", "Sto mangiando" } },
            };

            foreach (var question in questions)
            {
                await _connection.ExecuteAsync(
                    "INSERT INTO public.\"Questions\" (\"QuizID\", \"QuestionText\", \"Answer\", \"Choices\") VALUES (@QuizID, @QuestionText, @Answer, @Choices)",
                    new
                    {
                        question.QuizID,
                        question.QuestionText,
                        question.Answer,
                        Choices = question.Choices.ToArray() 
                    }
                );
            }

        }
    }
}
