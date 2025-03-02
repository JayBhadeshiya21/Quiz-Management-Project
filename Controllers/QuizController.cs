using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Quiz1.Models;

namespace Quiz1.Controllers
{
    public class QuizController : Controller
    {
        private IConfiguration configuration;

        public QuizController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult Add_Quiz(AddQuizModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;


                command.CommandText = "PR_Quiz_Insert";
                command.Parameters.Add("@QuizName", SqlDbType.VarChar).Value = model.QuizName;
                command.Parameters.Add("@QuizDate", SqlDbType.DateTime).Value = model.QuizDate;
                command.Parameters.Add("@UserId", SqlDbType.Int).Value = model.UserId;
                command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = model.Modified;
                command.Parameters.Add("@TotalQuestions", SqlDbType.Int).Value = model.TotalQuestions;
                command.ExecuteNonQuery();
                return RedirectToAction("Quiz_List");
            }

            return View("Add_Quiz", model);
        }

        public IActionResult Quiz_List()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Quiz_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult QuizDelete(int QuizID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Quiz_DeleteByPK";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = QuizID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Quiz deleted successfully.";
                return RedirectToAction("Quiz_List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the quiz: " + ex.Message;
                return RedirectToAction("Quiz_List");
            }
        }

        public IActionResult Form4()
        {
            return View();
        }
    }
}
