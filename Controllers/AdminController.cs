using TaskCollabration.Models;
using Microsoft.AspNetCore.Mvc;

namespace TaskCollabration.Controllers
{
    public class AdminController : Controller
    {
        AdminModel adminModel = new AdminModel();
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Team()
        {
            return View();
        }

        public IActionResult Report()
        {
            return View();
        }

        public IActionResult Project()
        {
            return View();
        }

        public IActionResult Task()
        {
            return View();
        }

        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUser(AdminModel admin)
        {
            bool result;
            if (ModelState.IsValid)
            {
                adminModel = new AdminModel();
                result = adminModel.insert(admin);
                if (result)
                {
                    TempData["msg"] = "Added Successfully";
                }
                else
                {
                    TempData["msg"] = "Failed To Add Data";
                }
            }
            return View();
        }
        public IActionResult ListUser()
        {
            adminModel = new AdminModel();
            List<AdminModel> adminModels= adminModel.getData();
            return View(adminModels);
        }


        public IActionResult EditAdminProfile()
        {
            return View();
        }
    }
}
