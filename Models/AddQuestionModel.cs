using System.ComponentModel.DataAnnotations;

namespace Quiz1.Models
{
    public class AddQuestionModel
    {
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
        public string QuestionMarks { get; set; }

      
    }
}
