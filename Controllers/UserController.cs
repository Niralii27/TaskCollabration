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
        FetchTaskUserModel fetchTaskUserModel = new FetchTaskUserModel();
        ViewModel viewmodel = new ViewModel();
        TeamModel teamModel = new TeamModel();
        SettingModel settingModel = new SettingModel();
        ReportsModel reportsModel = new ReportsModel();
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
            // User ID ko session se lena
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth"); // Redirect to Login if session is empty
            }

            if (!int.TryParse(id, out int projectId))
            {
                return BadRequest("Invalid Project ID"); // Handle invalid conversion
            }

            MessageModel messageModel = new MessageModel();
            List<MessageModel> projectMessages = messageModel.GetProjectMessages(projectId); // Use projectId (int) here
            ViewBag.UserID = userId;
            ViewBag.ProjectId = id;
            ViewBag.ProjectMessages = projectMessages;

            var viewModel = new ViewModel
            {
                Projects = userprojectModel.getData(id), // Keep using id (string) here
                Tasks = fetchTaskUserModel.getdata1(userId.Value, projectId)
            };

            return View(viewModel);
        }

        //Fetch data from a Team Table
        public IActionResult Team()
        {
            teamModel = new TeamModel();
            List<TeamModel> teamModels = teamModel.getData();
            return View(teamModels);
        }
        public IActionResult Reports()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            //  getdata
            ReportsModel reportsModel = new ReportsModel();
            List<ReportsModel> tasks = reportsModel.getdata(userId.Value);

            // ViewBag
            ViewBag.TotalTasks = tasks.Count;
            ViewBag.CompletedTasks = tasks.Count(t => t.Status.Equals("Completed", StringComparison.OrdinalIgnoreCase));
            ViewBag.PendingTasks = tasks.Count(t => t.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase));
            ViewBag.InProgressTasks = tasks.Count(t => t.Status.Equals("In Progress", StringComparison.OrdinalIgnoreCase));
            ViewBag.DelayedTasks = tasks.Count(t => t.Status.Equals("Delayed", StringComparison.OrdinalIgnoreCase));

            // For the weekly chart - tasks completed per day this week
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek);

            var weeklyData = new int[7];
            var newTasksData = new int[7];

            for (int i = 0; i < 7; i++)
            {
                var currentDay = startOfWeek.AddDays(i);
                // Count completed tasks for this day
                weeklyData[i] = tasks.Count(t => t.Status.Equals("Completed") &&
                                                 t.Date.Date == currentDay);

                // Count new tasks created on this day
                newTasksData[i] = tasks.Count(t => t.Date.Date == currentDay);
            }

            ViewBag.WeeklyLabels = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            ViewBag.WeeklyCompletedData = weeklyData;
            ViewBag.WeeklyNewTasksData = newTasksData;

            // For the distribution chart
            ViewBag.DistributionData = new int[]
  {
    ViewBag.CompletedTasks,
    ViewBag.InProgressTasks,
    ViewBag.PendingTasks,
    ViewBag.DelayedTasks
  };

            return View();
        }
        [HttpGet]
        [Route("User/Setting/{id}")]  // Specific route for ID-based retrieval
        public IActionResult SettingById(string id)
        {
            SettingModel user = settingModel.getData(id);
            return View(user);
        }

        [HttpGet]
        [Route("User/Setting")]  // Default route without ID
        public IActionResult Setting()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UserID = userId;
            SettingModel settingModel = new SettingModel();
            List<SettingModel> users = settingModel.getdata(userId.Value);

            var viewModel = new SettingModel
            {
                SettingList = users ?? new List<SettingModel>()
            };

            return View(viewModel);
        }


        //Update a Setting Data

        [HttpPost]
        public IActionResult Setting(SettingModel model)
        {
            // Don't create a new empty model here
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            model.Id = userId.Value; // Set the user ID from session
            bool res = model.update(model);

            if (res)
            {
                TempData["msg"] = "Updated Successfully";
                return RedirectToAction("Setting");
            }
            else
            {
                TempData["msg"] = "Failed to Update Data";
            }
            return View(model);
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
