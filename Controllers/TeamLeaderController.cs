using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using TaskCollabration.Models;

namespace TaskCollabration.Controllers
{
    public class TeamLeaderController : Controller
    {
        TeamLeaderModel teamLeadermodel = new TeamLeaderModel();
        AddUserTaskModel addUserTaskModel = new AddUserTaskModel();
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
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UserID = userId;

            TeamLeaderModel teamLeadermodel = new TeamLeaderModel();

            List<TeamLeaderModel> team = teamLeadermodel.getdata(userId.Value);

            var viewModel = new TeamLeaderModel
            {
                UsersList = team ?? new List<TeamLeaderModel>()
            };

            return View(viewModel);
        }

        public IActionResult TTeam()
        {
            return View();
        }
        [HttpPost]
        public IActionResult TTask(TeamLeaderModel user1, IFormFile formFile, [FromServices] IWebHostEnvironment hostEnvironment)
        {
            int? userId = HttpContext.Session.GetInt32("UserID");

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            user1.UserID = userId.Value;

            try
            {
                if (formFile != null && formFile.Length > 0)
                {
                    // Get the uploads folder path
                    string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "uploads");

                    // Ensure directory exists
                    Directory.CreateDirectory(uploadsFolder);

                    // Use the original filename directly
                    string fileName = formFile.FileName;

                    // Full path for saving the file
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(fileStream);
                    }

                    // Store the relative path in the database
                    user1.FilePath = Path.Combine("uploads", fileName);
                }
                bool result = user1.insert(user1);

                if (result)
                {
                    TempData["SuccessMessage"] = "Task added successfully!";
                    return RedirectToAction("TTask");
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add task.";
                    return View(user1);
                }
            }

            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return View(user1);
            }

        }


        public IActionResult AddUserTask()
        {
            addUserTaskModel = new AddUserTaskModel();
            List<AddUserTaskModel> users = addUserTaskModel.getData();
            var viewModel = new AddUserTaskModel
            {
                UsersList = users ?? new List<AddUserTaskModel>()
            };

            return View(viewModel);
        }

            [HttpPost]
            public IActionResult AddUserTask(AddUserTaskModel user1, IFormFile formFile, [FromServices] IWebHostEnvironment hostEnvironment)
            {
        

                try
                {
                    if (formFile != null && formFile.Length > 0)
                    {
                        // Get the uploads folder path
                        string uploadsFolder = Path.Combine(hostEnvironment.WebRootPath, "uploads");

                        // Ensure directory exists
                        Directory.CreateDirectory(uploadsFolder);

                        // Use the original filename directly
                        string fileName = formFile.FileName;

                        // Full path for saving the file
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        // Save the file
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            formFile.CopyTo(fileStream);
                        }

                        // Store the relative path in the database
                        user1.FilePath = Path.Combine(fileName);
                    }

                    // Continue with your existing insertion logic
                    bool result = user1.insert(user1);

                    if (result)
                    {
                        TempData["SuccessMessage"] = "Task added successfully!";
                        return RedirectToAction("AddUserTask");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to add task.";
                        return View(user1);
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                    return View(user1);
                }
            }


    }

}
