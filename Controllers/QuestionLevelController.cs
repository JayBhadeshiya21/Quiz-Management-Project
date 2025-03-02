using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Quiz1.Models;

namespace Quiz1.Controllers
{
    public class QuestionLevelController : Controller
    {

        private IConfiguration configuration;

        public QuestionLevelController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult ListQuestionLevel()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_QuestionLevel_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult QuestionLevelsDelete(int QuestionLevels)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_QuestionLevel_DeleteByPK";
                    command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = QuestionLevels;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "QuestionLevel deleted successfully.";
                return RedirectToAction("QuestionLevel");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the QuestionLevel: " + ex.Message;
                return RedirectToAction("QuestionLevel");
            }
        }

        public IActionResult AddQuestionLevel()
        {
            return View();
        }

        public IActionResult QuestionLevelsInsert(AddQuestionLevelModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;


                command.CommandText = "PR_QuestionLevel_Insert";
                command.Parameters.Add("@QuestionLevel", SqlDbType.VarChar).Value = model.QuestionLevel;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserId;
                command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = model.Modified;
                command.ExecuteNonQuery();
                return RedirectToAction("ListQuestionLevel");
            }

            return View("AddQuestionLevel", model);
        }
    }
}
