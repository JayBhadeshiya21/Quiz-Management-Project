using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Quiz1.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Quiz1.Controllers
{
    [CheckAccess]
    public class HomeController : Controller
    {

        private IConfiguration configuration;

        public HomeController(IConfiguration _configuration)
        {
            this.configuration = _configuration;
        }

        public IActionResult Index()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");

            // Create DataTables for quizzes and questions
            DataTable quizTable = new DataTable();
            DataTable questionTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Get recent quizzes
                using (SqlCommand quizCommand = new SqlCommand("PR_TOP10_QUIZ", connection))
                {
                    quizCommand.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader quizReader = quizCommand.ExecuteReader())
                    {
                        quizTable.Load(quizReader);
                    }
                }

                // Get recent questions
                using (SqlCommand questionCommand = new SqlCommand("PR_TOP10_QUESTION", connection))
                {
                    questionCommand.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader questionReader = questionCommand.ExecuteReader())
                    {
                        questionTable.Load(questionReader);
                    }
                }
            }

            // Pass both tables to ViewData
            ViewData["RecentQuizzes"] = quizTable;
            ViewData["RecentQuestions"] = questionTable;

            return View();
        }

        public IActionResult QuizList()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_Quiz_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]



        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class CheckAccess : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext.HttpContext.Session.GetString("UserID") == null)
            {
                filterContext.Result = new RedirectResult("~/User/Login");
            }
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            context.HttpContext.Response.Headers["Expires"] = "-1";
            context.HttpContext.Response.Headers["Pragma"] = "no-cache";
            base.OnResultExecuting(context);
        }
    }
}
