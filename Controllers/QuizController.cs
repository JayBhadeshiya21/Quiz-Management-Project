using Microsoft.AspNetCore.Mvc;

namespace Quiz1.Controllers
{
    public class QuizController : Controller
    {
        public IActionResult Add_Quiz()
        {
            return View();
        }

        //public IActionResult Update_Quiz()
        //{
        //    return View();
        //}

        public IActionResult Quiz_List()
        {
            return View();
        }

        public IActionResult Form4()
        {
            return View();
        }
    }
}
