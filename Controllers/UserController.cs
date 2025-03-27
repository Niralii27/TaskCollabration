using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using TaskCollabration.Models;

namespace TaskCollabration.Controllers
{
    public class UserController : Controller
    {
        UserModel usermodel = new UserModel();
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Task()
        {
            int? userId = HttpContext.Session.GetInt32("UserID"); // Session से ID प्राप्त करें

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth"); // अगर सेशन में ID नहीं है, तो लॉगिन पर भेजें
            }

            ViewBag.UserID = userId;

            UserModel usermodel = new UserModel(); 

            List<UserModel> users = usermodel.getdata(userId.Value); // Fetch user tasks

            var viewModel = new UserModel
            {
                UsersList = users ?? new List<UserModel>() // Avoid null
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Task(UserModel user1)
        {
            int? userId = HttpContext.Session.GetInt32("UserID"); 

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth"); 
            }

            user1.UserID = userId.Value;

            bool res;
            if(ModelState.IsValid)
            {
                usermodel= new UserModel();
                res = usermodel.insert(user1);
                if(res)
                {
                    TempData["msg"] = "Task Added Successfully!!!!!";
                    return RedirectToAction("Task"); // Redirect on success

                }
                else
                {
                    TempData["msg"] = "Failed To add Task";
                }
            }
            return View();
        }
        public IActionResult Project()
        {
            return View();
        }

        public IActionResult Team()
        {
            return View();
        }

        public IActionResult Reports()
        {
            return View();
        }

        public IActionResult Setting()
        {
            return View();
        }

        
    }
}
