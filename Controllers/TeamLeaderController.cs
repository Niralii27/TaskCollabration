using Microsoft.AspNetCore.Mvc;

namespace TaskCollabration.Controllers
{
    public class TeamLeaderController : Controller
    {
        public IActionResult THome()
        {
            return View();
        }
    }
}
