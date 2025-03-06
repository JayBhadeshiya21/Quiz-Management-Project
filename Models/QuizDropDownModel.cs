using Microsoft.AspNetCore.Mvc;

namespace Quiz1.Models
{
    public class QuizDropDownModel : Controller
    {
        public int QuizID { get; set; }
        public string QuizName { get; set; }
    }
}
