using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Quiz1.Models;

namespace Quiz1.Controllers
{
    public class QuizWiseQuestionController : Controller
    {
        private IConfiguration configuration;

        public QuizWiseQuestionController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult ListQuizWiseQuestions()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_QuizWiseQuestions_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult AddQuizWiseQuestions()
        {
            return View();
        }
        public IActionResult QuizWiseQuestionsDetails()
        {
            return View();
        }


        public IActionResult QuizwisequestionInsert(AddQuizwisequestion model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;


                command.CommandText = "PR_QuizWiseQuestions_Insert";
                command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizId;
                command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionId;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserId;
                command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = model.Modified;
                command.ExecuteNonQuery();
                return RedirectToAction("ListQuizWiseQuestions");
            }

            return View("AddQuizWiseQuestions", model);
        }


    }
}
