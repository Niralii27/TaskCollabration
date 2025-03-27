using Microsoft.AspNetCore.Mvc;
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
            UserModel usermodel = new UserModel(); // Ensure it's properly instantiated
            List<UserModel> users = usermodel.getdata(); // Fetch user tasks

            var viewModel = new UserModel
            {
                UsersList = users ?? new List<UserModel>() // Avoid null
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Task(UserModel user1)
        {
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
