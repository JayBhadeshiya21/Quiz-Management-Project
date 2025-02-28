using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;

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
    }
}
