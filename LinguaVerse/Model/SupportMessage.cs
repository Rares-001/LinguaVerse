using System.ComponentModel.DataAnnotations;

namespace LinguaVerseApi.Models
{
    public class SupportMessage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }
        
        public string Message { get; set; }
    }
}
