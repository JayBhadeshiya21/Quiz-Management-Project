using System.ComponentModel.DataAnnotations;

namespace Quiz1.Models
{
    public class AddQuestionLevelModel
    {
        [Required]
        public string QuestionLevel { get; set; }
    }
}
