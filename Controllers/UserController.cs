using Microsoft.AspNetCore.Mvc;

namespace TaskCollabration.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult Task()
        {
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
