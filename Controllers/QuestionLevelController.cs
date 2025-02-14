using Microsoft.AspNetCore.Mvc;

namespace Quiz1.Controllers
{
    public class QuestionLevelController : Controller
    {
        public IActionResult ListQuestionLevel()
        {
            return View();
        }

        public IActionResult AddQuestionLevel()
        {
            return View();
        }
    }
}
