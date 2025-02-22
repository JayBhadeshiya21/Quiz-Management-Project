using Microsoft.AspNetCore.Mvc;

namespace Quiz1.Controllers
{
    public class QuizWiseQuestionController : Controller
    {

        public IActionResult ListQuizWiseQuestions()
        {
            return View();
        }

        public IActionResult AddQuizWiseQuestions()
        {
            return View();
        }
        public IActionResult QuizWiseQuestionsDetails()
        {
            return View();
        }
    }
}
