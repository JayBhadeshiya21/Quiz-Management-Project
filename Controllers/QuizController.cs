using System.Data;
using System.Data.SqlClient;
using Quiz1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;

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
            UserDropDown();
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                command.CommandText = "PR_Quiz_Insert";
                UserDropDown();
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

        public IActionResult AddAndEdit(AddQuizModel model)
        {
            UserDropDown();
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (model.QuizId == 0)
                {
                    command.CommandText = "PR_Quiz_Insert";
                }
                else
                {
                    command.CommandText = "PR_Quiz_UpdateByPK";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizId;
                }
                UserDropDown();
                command.Parameters.Add("@QuizName", SqlDbType.VarChar).Value = model.QuizName;
                command.Parameters.Add("@TotalQuestions", SqlDbType.Int).Value = model.TotalQuestions;
                command.Parameters.Add("@QuizDate", SqlDbType.DateTime).Value = model.QuizDate;
                command.Parameters.Add("@Modified", SqlDbType.DateTime).Value = model.Modified;
                command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserId;
                command.ExecuteNonQuery();
                return RedirectToAction("Quiz_List");
            }

            return View("Add_Quiz", model);
        }

        public IActionResult Quiz_From_edit(int? QuizId)
        {
            UserDropDown();
            if (QuizId == null)
            {
                return View(new AddQuizModel { Modified = DateTime.Now });
            }
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("PR_Quiz_SelectByPK", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@QuizID", QuizId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            var model = new AddQuizModel
                            {
                                QuizName = reader["QuizName"].ToString(),
                                TotalQuestions = Convert.ToInt32(reader["TotalQuestions"]),
                                QuizDate = Convert.ToDateTime(reader["QuizDate"]),
                                UserId = Convert.ToInt32(reader["UserID"]),
                                Modified = Convert.ToDateTime(reader["Modified"])
                            };
                            return View(model);
                        }
                    }
                }
            }
            return View(new AddQuizModel()); // Return an empty model if no data found
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
            List<QuizDropDownModel> Userlist = new List<QuizDropDownModel>();
            foreach (DataRow data in dataTable2.Rows)
            {
                QuizDropDownModel model = new QuizDropDownModel();
                model.UserID = Convert.ToInt32(data["UserID"]);
                model.UserName= data["UserName"].ToString();
                Userlist.Add(model);
            }
            ViewBag.Userlist = Userlist;
        }

        public IActionResult ExportToExcel()
        {
            string connectionString = configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_Quiz_SelectAll";

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(sqlDataReader);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cells[1, 1].Value = "QuizID";
                worksheet.Cells[1, 2].Value = "QuizName";
                worksheet.Cells[1, 3].Value = "UserName";
                worksheet.Cells[1, 4].Value = "TotalQuestions";
                worksheet.Cells[1, 5].Value = "QuizDate";

                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cells[row, 1].Value = item["QuizID"];
                    worksheet.Cells[row, 2].Value = item["QuizName"];
                    worksheet.Cells[row, 3].Value = item["UserName"];
                    worksheet.Cells[row, 4].Value = item["TotalQuestions"];
                    worksheet.Cells[row, 5].Value = item["QuizDate"];
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"Data-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
    }
}
