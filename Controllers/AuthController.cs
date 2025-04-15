using TaskCollabration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace TaskCollabration.Controllers
{
    public class AuthController : Controller
    {
        AuthModel authModel = new AuthModel();

        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=Task;Integrated Security=True;";

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Demo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AuthModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT * FROM Users WHERE Email = @Email AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@Password", model.Password); // 🔹 Hash passwords in real applications

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int userId = Convert.ToInt32(reader["Id"]);
                    string FirstName = (reader["FirstName"].ToString());
                    string Image = (reader["Image"].ToString());

                    string type = reader["Type"].ToString();


                    HttpContext.Session.SetInt32("UserID", userId);
                    HttpContext.Session.SetString("Type", type);
                    HttpContext.Session.SetString("Email", model.Email);
                    HttpContext.Session.SetString("FirstName", FirstName);
                    HttpContext.Session.SetString("Image", Image);


                    HttpContext.Session.SetString("Type", type);

                    switch (type)
                    {
                        case "Admin":
                            return RedirectToAction("Dashboard", "Admin");
                        case "Team Leader":
                            return RedirectToAction("THome", "TeamLeader");
                        case "User":
                            return RedirectToAction("Home", "User");
                        default:
                            return RedirectToAction("Login", "Auth");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
    }
}
