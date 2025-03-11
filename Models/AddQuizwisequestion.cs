using System.ComponentModel.DataAnnotations;

namespace Quiz1.Models
{
    public class AddQuizwisequestion
    {
        [Required]
        public int QuizwisequestionId {  get; set; }

        [Required]
        public int QuizId { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime Modified { get; set; }
    }
}
