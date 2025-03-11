using System.ComponentModel.DataAnnotations;

namespace Quiz1.Models
{
    public class AddQuestionLevelModel
    {
        [Required]
        public int QuestionLevelID { get; set; }
        [Required]
        public string QuestionLevel { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime Modified { get; set; }
    }
}
