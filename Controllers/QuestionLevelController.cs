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

        public IActionResult QuestionLevelsDelete(int QuestionLevelID)
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
                    command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = QuestionLevelID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "QuestionLevel deleted successfully.";
                return RedirectToAction("ListQuestionLevel");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the QuestionLevel: " + ex.Message;
                return RedirectToAction("ListQuestionLevel");
            }
        }

        public IActionResult AddQuestionLevel()
        {
            UserDropDown();
            return View();
        }

        //public IActionResult QuestionLevelsInsert(AddQuestionLevelModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string connectionString = this.configuration.GetConnectionString("ConnectionString");
        //        SqlConnection connection = new SqlConnection(connectionString);
        //        connection.Open();
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandType = CommandType.StoredProcedure;


        //        command.CommandText = "PR_QuestionLevel_Insert";
        //        command.Parameters.Add("@QuestionLevel", SqlDbType.VarChar).Value = model.QuestionLevel;
        //        command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserId;
        //        command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = model.Modified;
        //        command.ExecuteNonQuery();
        //        return RedirectToAction("ListQuestionLevel");
        //    }

        //    return View("AddQuestionLevel", model);
        //}

        public IActionResult AddAndEdit(AddQuestionLevelModel model)
        {
            UserDropDown();
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuestionLevelID == 0)
                {
                    command.CommandText = "PR_QuestionLevel_Insert";
                }
                else
                {
                    command.CommandText = "PR_QuestionLevel_UpdateByPK";
                    command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = model.QuestionLevelID;
                }
                command.Parameters.Add("@QuestionLevel", SqlDbType.VarChar).Value = model.QuestionLevel;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = CommonVariable.UserID();
                command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = model.Modified;
                command.ExecuteNonQuery();
                return RedirectToAction("ListQuestionLevel");
            }

            return View("AddQuestionLevel", model);
        }

        public IActionResult QuestionLevel_From_edit(int? QuestionLevelID)
        {
            UserDropDown();
            if (QuestionLevelID == null)
            {
                return View(new AddQuizModel { Modified = DateTime.Now });
            }

            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_QuestionLevel_SelectByPK", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@QuestionLevelID", QuestionLevelID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            var model = new AddQuestionLevelModel
                            {
                                QuestionLevel = reader["QuestionLevel"].ToString(),
                                UserId = Convert.ToInt32(reader["UserID"]),
                                Modified = Convert.ToDateTime(reader["Modified"])
                            };
                            return View(model);
                        }
                    }
                }
            }
            return View(new AddQuestionLevelModel()); // Return an empty model if no data found
        }

        public void UserDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_MST_QUIZ_DROPDWON";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<QuizLevel_Dropdown> Userlist = new List<QuizLevel_Dropdown>();
            foreach (DataRow data in dataTable2.Rows)
            {
                QuizLevel_Dropdown model = new QuizLevel_Dropdown();
                model.UserID = Convert.ToInt32(data["UserID"]);
                model.UserName = data["UserName"].ToString();
                Userlist.Add(model);
            }
            ViewBag.Userlist = Userlist;
        }
    }
}
