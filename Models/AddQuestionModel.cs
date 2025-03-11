using System.ComponentModel.DataAnnotations;

namespace Quiz1.Models
{
    public class AddQuestionModel
    {
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public string QuestionText { get; set; }

        [Required]
        public int QuestionLevelID { get; set; }

        [Required]
        public string OptionA { get; set; }

        [Required]
        public string OptionB { get; set; }

        [Required]
        public string OptionC { get; set; }

        [Required]
        public string OptionD { get; set; }

        [Required]
        public string CorrectOption { get; set; }

        [Required]
        public int QuestionMarks { get; set; }

        [Range(0,1)]
        public int IsActive { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        public DateTime Modified { get; set; }


    }
}
