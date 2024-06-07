using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinguaVerse.Model
{
    public class TestQuestion
    {
        public string QuestionText { get; set; }
        public ObservableCollection<string> Choices { get; set; }
        public string CorrectAnswer { get; set; }
        public string SelectedAnswer { get; set; }
    }
}
