using Microsoft.AspNetCore.Mvc;

namespace TaskCollabration.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
