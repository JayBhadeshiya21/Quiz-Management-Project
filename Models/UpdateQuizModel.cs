using System.ComponentModel.DataAnnotations;

namespace Quiz1.Models
{
    public class UpdateQuizModel
    {

        [Required]
        public int QuizId { get; set; }

        [Required]
        public string NewQuizName { get; set; }

        [Required]
        public int TotalQuestion { get; set; }

        [Required]
        public DateTime QuizDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }



    }
}
