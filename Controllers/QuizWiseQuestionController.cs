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
            UserDropDown();
            QuizDropDown();
            QuestionDropDown();
            return View();
        }
        public IActionResult QuizWiseQuestionsDetails()
        {
            return View();
        }

        //public IActionResult QuizwisequestionInsert(AddQuizwisequestion model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string connectionString = this.configuration.GetConnectionString("ConnectionString");
        //        SqlConnection connection = new SqlConnection(connectionString);
        //        connection.Open();
        //        SqlCommand command = connection.CreateCommand();
        //        command.CommandType = CommandType.StoredProcedure;


        //        command.CommandText = "PR_QuizWiseQuestions_Insert";
        //        command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizId;
        //        command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionId;
        //        command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserId;
        //        command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = model.Modified;
        //        command.ExecuteNonQuery();
        //        return RedirectToAction("ListQuizWiseQuestions");
        //    }

        //    return View("AddQuizWiseQuestions", model);
        //}

        public IActionResult AddAndEdit(AddQuizwisequestion model)
        {
            UserDropDown();
            QuizDropDown();
            QuestionDropDown();
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuizWiseQuestionsID == 0)
                {
                    command.CommandText = "PR_QuizWiseQuestions_Insert";
                }
                else
                {
                    command.CommandText = "PR_QuizWiseQuestions_UpdateByPK";
                    command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = model.QuizWiseQuestionsID;
                }
                command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizId;
                command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionId;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = CommonVariable.UserID();
                command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = model.Modified;
                command.ExecuteNonQuery();
                return RedirectToAction("ListQuizWiseQuestions");
            }

            return View("AddQuizWiseQuestions", model);
        }
        public IActionResult QuizWiseQuestions_From_edit(int? QuizWiseQuestionsID)
        {
            UserDropDown();
            QuizDropDown();
            QuestionDropDown();
            if (QuizWiseQuestionsID == null)
            {
                return View(new AddQuizModel { Modified = DateTime.Now });
            }
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_QuizWiseQuestions_SelectByPK", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@QuizWiseQuestionsID", QuizWiseQuestionsID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            var model = new AddQuizwisequestion
                            {
                                QuizId = Convert.ToInt32(reader["QuizID"]),
                                QuestionId = Convert.ToInt32(reader["QuestionID"]),
                                UserId = Convert.ToInt32(reader["UserID"]),
                                Modified = Convert.ToDateTime(reader["Modified"])
                            };
                            return View(model);
                        }
                    }
                }
            }
            return View(new AddQuizwisequestion()); // Return an empty model if no data found
        }

        public IActionResult QuizWiseQuestionsDelete(int? QuizWiseQuestionsID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_QuizWiseQuestions_DeleteByPK";
                    command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = QuizWiseQuestionsID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "QuestionLevel deleted successfully.";
                return RedirectToAction("ListQuizWiseQuestions");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the QuestionLevel: " + ex.Message;
                return RedirectToAction("ListQuizWiseQuestions");
            }
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
            List<AddWiseDropDwon> Userlist = new List<AddWiseDropDwon>();
            foreach (DataRow data in dataTable2.Rows)
            {
                AddWiseDropDwon model = new AddWiseDropDwon();
                model.UserID = Convert.ToInt32(data["UserID"]);
                model.UserName = data["UserName"].ToString();
                Userlist.Add(model);
            }
            ViewBag.Userlist = Userlist;
        }

        public void QuizDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_QUIZ_dropdwon";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<AddWiseDropDwon> Quizlist = new List<AddWiseDropDwon>();
            foreach (DataRow data in dataTable2.Rows)
            {
                AddWiseDropDwon model = new AddWiseDropDwon();
                model.QuizId = Convert.ToInt32(data["QuizID"]);
                model.QuizName= data["QuizName"].ToString();
                Quizlist.Add(model);
            }
            ViewBag.Quizlist = Quizlist;
        }

        public void QuestionDropDown()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_QUESTION_dropdwon";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            List<AddWiseDropDwon> Questionlist = new List<AddWiseDropDwon>();
            foreach (DataRow data in dataTable2.Rows)
            {
                AddWiseDropDwon model = new AddWiseDropDwon();
                model.QuestionId = Convert.ToInt32(data["QuestionID"]);
                model.QuestionName = data["QuestionText"].ToString();
                Questionlist.Add(model);
            }
            ViewBag.Questionlist = Questionlist;
        }
    }
}
