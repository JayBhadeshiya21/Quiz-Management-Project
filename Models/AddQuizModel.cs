using System.ComponentModel.DataAnnotations;

namespace Quiz1.Models
{
    public class AddQuizModel
    {

        [Required]
        public string QuizName { get; set; }

       
        [Required]
        public DateTime QuizDate { get; set; }

        [Required]
        public int TotalQuestions { get; set; }
    }
}
