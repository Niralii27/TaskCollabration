using Microsoft.AspNetCore.Mvc;

namespace TaskCollabration.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Demo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    HttpContext.Session.SetString("UserRole", user.Role);

                    switch (user.Role)
                    {
                        case "Admin":
                            return RedirectToAction("Dashboard", "Admin");
                        case "Team Leader":
                            return RedirectToAction("Dashboard", "TeamLeader");
                        case "User":
                            return RedirectToAction("Dashboard", "User");
                        default:
                            return RedirectToAction("Login", "Account");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }

            return View(model);
        }

    }
}
