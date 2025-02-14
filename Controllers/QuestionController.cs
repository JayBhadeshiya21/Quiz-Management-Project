using Microsoft.AspNetCore.Mvc;

namespace Quiz1.Controllers
{
    public class QuestionController : Controller
    {
  
        //public IActionResult QuestionLevel()
        //{
        //    return View();
        //}

        //public IActionResult AddQuestionLevel()
        //{
        //    return View();
        //}

        public IActionResult QuestionList()
        {
            return View();
        }

        public IActionResult AddQuestion()
        {
            return View();
        }


        //public IActionResult QuizWiseQuestions()
        //{
        //    return View();
        //}

        //public IActionResult AddQuizWiseQuestions()
        //{
        //    return View();
        //}

        //public IActionResult QuizWiseQuestionsDetails()
        //{
        //    return View();
        //}
    }
}
