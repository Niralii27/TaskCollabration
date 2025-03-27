using Microsoft.AspNetCore.Mvc;
using TaskCollabration.Models;

namespace TaskCollabration.Controllers
{
    public class TeamLeaderController : Controller
    {
        TeamLeaderModel teamLeadermodel = new TeamLeaderModel();
        public IActionResult THome()
        {
            return View();
        }

        public IActionResult TProject()
        {
            return View();
        }

        public IActionResult TTask()
        {
            int? userId = HttpContext.Session.GetInt32("UserID"); // Session से ID प्राप्त करें

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth"); // अगर सेशन में ID नहीं है, तो लॉगिन पर भेजें
            }

            ViewBag.UserID = userId;
            TeamLeaderModel teamleadermodel = new TeamLeaderModel(); // Ensure it's properly instantiated
            List<TeamLeaderModel> team = teamLeadermodel.getdata(userId.Value); // Fetch user tasks

            var viewModel = new TeamLeaderModel
            {
                UsersList = team ?? new List<TeamLeaderModel>() // Avoid null
            };

            return View(viewModel);
        }

        public IActionResult TTeam()
        {
            return View();
        }
        [HttpPost]
        public IActionResult TTask(TeamLeaderModel user1)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            user1.UserID = userId.Value;

            bool res;
            if (ModelState.IsValid)
            {
                teamLeadermodel = new TeamLeaderModel();
                res = teamLeadermodel.insert(user1);
                if (res)
                {
                    TempData["msg"] = "Task Added Successfully!!!!!";
                    return RedirectToAction("TTask"); // Redirect on success

                }
                else
                {
                    TempData["msg"] = "Failed To add Task";
                }
            }
            return View();
        }
    }
}
