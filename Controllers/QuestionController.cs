using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Quiz1.Models;

namespace Quiz1.Controllers
{
    public class QuestionController : Controller
    {
        private IConfiguration configuration;

        public QuestionController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        public IActionResult AddQuestion()
        {
            return View();
        }

        public IActionResult QuestionList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_Question_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }

        public IActionResult QuestionDelete(int QuestionID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_Question_DeleteByPK";
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = QuestionID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Question deleted successfully.";
                return RedirectToAction("QuestionList");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the Question: " + ex.Message;
                return RedirectToAction("QuestionList");
            }
        }

        //public IActionResult QuestionInsert(AddQuestionModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string connectionString = this.configuration.GetConnectionString("ConnectionString");
        //        SqlConnection connection = new SqlConnection(connectionString);
        //        connection.Open();
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandType = CommandType.StoredProcedure;

        //        command.CommandText = "PR_Question_Insert";
        //        command.Parameters.Add("@QuestionText", SqlDbType.VarChar).Value = model.QuestionText;
        //        command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = model.QuestionLevelID;
        //        command.Parameters.Add("@OptionA", SqlDbType.VarChar).Value = model.OptionA;
        //        command.Parameters.Add("@OptionB", SqlDbType.VarChar).Value = model.OptionB;
        //        command.Parameters.Add("@OptionC", SqlDbType.VarChar).Value = model.OptionC;
        //        command.Parameters.Add("@OptionD", SqlDbType.VarChar).Value = model.OptionD;
        //        command.Parameters.Add("@CorrectOption", SqlDbType.VarChar).Value = model.CorrectOption;
        //        command.Parameters.Add("@QuestionMarks", SqlDbType.Int).Value = model.QuestionMarks;
        //        command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = model.IsActive;
        //        command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
        //        command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = model.Modified;
        //        command.ExecuteNonQuery();
        //        return RedirectToAction("QuestionList");
        //    }

        //    return View("AddQuestion", model);
        //}

        public IActionResult QuestionInsert(AddQuestionModel model)
        {
            User_Dropdown();
            Level_Dropdown();
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuestionId == 0)
                {
                    command.CommandText = "PR_Question_Insert";
                }
                else
                {
                    command.CommandText = "PR_Question_UpdateByPK";
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionId;
                }
                User_Dropdown();
                Level_Dropdown();
                command.Parameters.Add("@QuestionText", SqlDbType.VarChar).Value = model.QuestionText;
                command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = model.QuestionLevelID;
                command.Parameters.Add("@OptionA", SqlDbType.VarChar).Value = model.OptionA;
                command.Parameters.Add("@OptionB", SqlDbType.VarChar).Value = model.OptionB;
                command.Parameters.Add("@OptionC", SqlDbType.VarChar).Value = model.OptionC;
                command.Parameters.Add("@OptionD", SqlDbType.VarChar).Value = model.OptionD;
                command.Parameters.Add("@CorrectOption", SqlDbType.VarChar).Value = model.CorrectOption;
                command.Parameters.Add("@QuestionMarks", SqlDbType.Int).Value = model.QuestionMarks;
                command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = model.IsActive;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
                command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = model.Modified;
                command.ExecuteNonQuery();
                return RedirectToAction("QuestionList");
            }

            return View("AddQuestion", model);
        }

        public IActionResult Question_From_edit(int? QuestionId)
        {
            User_Dropdown();
            Level_Dropdown();
            if (QuestionId == null)
            {
                return View(new AddQuizModel { Modified = DateTime.Now });
            }

            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Question_SelectByPK", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@QuestionId", QuestionId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            var model = new AddQuestionModel
                            {
                                QuestionText = reader["QuestionText"].ToString(),
                                QuestionLevelID = Convert.ToInt32(reader["QuestionLevelID"]),
                                OptionA = reader["OptionA"].ToString(),
                                OptionB = reader["OptionB"].ToString(),
                                OptionC = reader["OptionC"].ToString(),
                                OptionD = reader["OptionD"].ToString(),
                                CorrectOption = reader["CorrectOption"].ToString(),
                                QuestionMarks = Convert.ToInt32(reader["QuestionMarks"]),
                                IsActive = Convert.ToInt32(reader["IsActive"]),
                                UserID = Convert.ToInt32(reader["UserID"]),
                                Modified = Convert.ToDateTime(reader["Modified"])
                            };
                            return View(model);
                        }
                    }
                }
            }
            return View(new AddQuizModel()); // Return an empty model if no data found
        }

        public void User_Dropdown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_MST_QUESTION_DROPDWON_USER1";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<Question_DropDown> userlist = new List<Question_DropDown>();
            foreach (DataRow data in dataTable2.Rows)
            {
                Question_DropDown model = new Question_DropDown();
                model.UserID = Convert.ToInt32(data["UserID"]);
                model.UserName = Convert.ToString(data["UserName"]);
                userlist.Add(model);
            }
            ViewBag.userlist = userlist;
        }

        public void Level_Dropdown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_QUESTION_LEVELDROPDOWN";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<Question_DropDown> Levellist = new List<Question_DropDown>();
            foreach (DataRow data in dataTable2.Rows)
            {
                Question_DropDown model = new Question_DropDown();
                model.QuestionLevelId = Convert.ToInt32(data["QuestionLevelID"]);
                model.QuestionLevelName= Convert.ToString(data["QuestionLevel"]);
                Levellist.Add(model);
            }
            ViewBag.Levellist = Levellist;
        }
    }
}
