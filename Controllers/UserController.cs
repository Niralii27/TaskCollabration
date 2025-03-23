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
            return View();
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
