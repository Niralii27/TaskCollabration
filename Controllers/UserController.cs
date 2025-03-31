using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using TaskCollabration.Models;
namespace TaskCollabration.Controllers
{
    public class UserController : Controller
    {
        UserModel usermodel = new UserModel();
        OtherTaskModel otherTask1= new OtherTaskModel();
        UserProjectModel userprojectModel = new UserProjectModel();
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Task()
        {
            int? userId = HttpContext.Session.GetInt32("UserID"); 
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth"); 
            }
            ViewBag.UserID = userId;
            UserModel usermodel = new UserModel();
            List<UserModel> users = usermodel.getdata(userId.Value); 
            var viewModel = new UserModel
            {
                UsersList = users ?? new List<UserModel>() 
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Task(UserModel user1, IFormFile formFile, [FromServices] IWebHostEnvironment hostEnvironment)
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
                    user1.FilePath = Path.Combine(fileName);
                }

                // Continue with your existing insertion logic
                bool result = user1.insert(user1);

                if (result)
                {
                    TempData["SuccessMessage"] = "Task added successfully!";
                    return RedirectToAction("Task");
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

        //Fetch Project from The Projects Table
        public IActionResult Project()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.UserID = userId;
            userprojectModel = new UserProjectModel();
            List<UserProjectModel> projects = userprojectModel.getData2(userId.Value);
            return View(projects);
        }

        public IActionResult ProjectDetails(string id)
        {
            UserProjectModel project = userprojectModel.getData(id);
            return View(project);
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

        public IActionResult OtherTask()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.UserID = userId;
            OtherTaskModel otherTask1 = new OtherTaskModel();
            List<OtherTaskModel> users = otherTask1.getdata(userId.Value);
            var viewModel = new OtherTaskModel
            {
                UsersList = users ?? new List<OtherTaskModel>()
            };
            return View(viewModel);
        }

        //Update a PersonalTask

        [HttpPost]
        public IActionResult EditTask(UserModel teamLeader, IFormFile formFile)
        {
            bool res;
            if (!ModelState.IsValid)
            {
                // Handle file upload if file is provided
                if (formFile != null && formFile.Length > 0)
                {
                    // Create uploads directory if it doesn't exist
                    string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Use original filename
                    string fileName = Path.GetFileName(formFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, fileName);

                    // Save the file
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(fileStream);
                    }

                    // Save the file path to your model
                    teamLeader.FilePath = fileName;
                }

                usermodel = new UserModel();
                res = usermodel.update(teamLeader);
                if (res)
                {
                    TempData["msg"] = "Updated Successfully";
                    return RedirectToAction("Task");
                }
                else
                {
                    TempData["msg"] = "Failed to UpdateData";
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult EditTask(string id)
        {
            UserModel user = usermodel.getData(id);
            return View(user);
        }

        //Delete TeamLeader PersonalTask

        [HttpPost]

        public IActionResult DeleteTask(UserModel leaderModel)
        {
            bool res;
            usermodel = new UserModel();
            res = usermodel.delete(leaderModel);
            if (res)
            {
                TempData["msg"] = "Deleted Successfully";
            }
            else
            {
                TempData["msg"] = "Not Deleted , Something went wrong!!!!!";
            }
            return View();
        }

        [HttpGet]
        public IActionResult DeleteTask(string id)
        {
            UserModel user = usermodel.getData(id);
            return View(user);
        }

        //Update a Other Task

        [HttpPost]
        public IActionResult EditOtherTask(OtherTaskModel teamLeader)
        {
            bool res;
            

                otherTask1 = new OtherTaskModel();
                res = otherTask1.update1(teamLeader);
                if (res)
                {
                    TempData["msg"] = "Updated Successfully";
                    return RedirectToAction("OtherTask");
                }
                else
                {
                    TempData["msg"] = "Failed to UpdateData";
                }
            
            return View();
        }
        [HttpGet]
        public IActionResult EditOtherTask(string id)
        {
            OtherTaskModel user = otherTask1.getData(id);
            return View(user);
        }

    }
}
