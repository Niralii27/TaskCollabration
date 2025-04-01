using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Runtime.Intrinsics.X86;
using TaskCollabration.Models;

namespace TaskCollabration.Controllers
{
    public class TeamLeaderController : Controller
    {
        TeamLeaderModel teamLeadermodel = new TeamLeaderModel();
        AddUserTaskModel addUserTaskModel = new AddUserTaskModel();
        ProjectModel projectmodel = new ProjectModel();
        MessageModel messageModel = new MessageModel();
        public IActionResult THome()
        {
            return View();
        }

        public IActionResult TProject1()
        {
            projectmodel= new ProjectModel();
            List<ProjectModel> projects = projectmodel.getData2();
            return View(projects);
        }
         public IActionResult TProjectDetails()
        {
            return View();
        }
      

        [HttpGet]
        public IActionResult TProjectDetails(string id)
        {
            ProjectModel project = projectmodel.getData(id);
            return View(project);
        }

        [HttpPost]

        public IActionResult TProjectDetails(MessageModel message)
        {
            bool result = message.insert(message);

            if (result)
            {
                TempData["SuccessMessage"] = "Task added successfully!";
                return RedirectToAction("TProjectDetails");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to add task.";
            }
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
        //Fetch the Users Data in EditProjectDetails

        public IActionResult AddMemberProject()
        {
            ProjectModel projectmodel = new ProjectModel();

            List<ProjectModel> team = projectmodel.getData1();

            var viewModel = new ProjectModel
            {
                UsersList = team ?? new List<ProjectModel>()
            };

            return View(viewModel);
        }

        //Update A Members

        [HttpPost]
        public IActionResult AddMemberProject(ProjectModel projectModel)
        {
            bool res;

            projectmodel = new ProjectModel();
            res = projectModel.update1(projectModel);
            if (res)
            {
                TempData["msg"] = "Updated Successfully!!!!!";
                return RedirectToAction("TProjectDetails");
            }
            else
            {
                TempData["msg"] = "Failed to UpdateData";
            }

            return View();
        }

        //Update ProjectDetails
        [HttpPost]
        public IActionResult EditProjectDetails(ProjectModel projectModel)
        {
            bool res;
            
                projectmodel = new ProjectModel();
                res = projectModel.update(projectModel);
                if (res)
                {
                    TempData["msg"] = "Updated Successfully!!!!!";
                    return RedirectToAction("TProject1");
                }
                else
                {
                    TempData["msg"] = "Failed to UpdateData";
                }
            
            return View();
        }
        [HttpGet]
        public IActionResult EditProjectDetails(string id)
        {
            ProjectModel projectModel = projectmodel.getData(id);
            return View(projectModel);
        }

        //Update a PersonalTask
        [HttpPost]
        public IActionResult EditPersonalTask(TeamLeaderModel teamLeader, IFormFile formFile)
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

                teamLeadermodel = new TeamLeaderModel();
                res = teamLeadermodel.update(teamLeader);
                if (res)
                {
                    TempData["msg"] = "Updated Successfully";
                    return RedirectToAction("TTask");
                }
                else
                {
                    TempData["msg"] = "Failed to UpdateData";
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult EditPersonalTask(string id)
        {
            TeamLeaderModel teamLeaderModel = teamLeadermodel.getData(id);
            return View(teamLeaderModel);
        }

        public IActionResult AddUserTask()
        {
            var addUserTaskModel = new AddUserTaskModel();

            List<AddUserTaskModel> users = addUserTaskModel.getData();
            List<AddUserTaskModel> users1 = addUserTaskModel.getData1();

            var viewModel = new AddUserTaskModel
            {
                UsersList = users ?? new List<AddUserTaskModel>(),
                UsersList1 = users1 ?? new List<AddUserTaskModel>() // Store users1 separately
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
        //Delete TeamLeader PersonalTask

        [HttpPost]

        public IActionResult DeletePersonalTask(TeamLeaderModel leaderModel)
        {
            bool res;
            teamLeadermodel = new TeamLeaderModel();
            res = teamLeadermodel.delete(leaderModel);
            if(res)
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
        public IActionResult DeletePersonalTask(string id)
        {
            TeamLeaderModel teamLeaderModel = teamLeadermodel.getData(id);
            return View(teamLeaderModel);
        }

        public IActionResult UsersTask()
        {
            addUserTaskModel = new AddUserTaskModel();

            List<AddUserTaskModel> team = addUserTaskModel.getdata();

            return View(team);
        }

        // Update Users Task From TeamLeader
        [HttpPost]
        public IActionResult EditUsersTask(AddUserTaskModel addUserTask, IFormFile formFile)
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
                    addUserTask.FilePath = fileName;
                }

                addUserTaskModel = new AddUserTaskModel();
                res = addUserTask.update1(addUserTask);
                if (res)
                {
                    TempData["msg"] = "Updated Successfully";
                    return RedirectToAction("UsersTask");
                }
                else
                {
                    TempData["msg"] = "Failed to UpdateData";
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult EditUsersTask(string id)
        {
            AddUserTaskModel addUserTaskmodel = addUserTaskModel.getData(id);
            return View(addUserTaskmodel);
        }

        //Delete UsersTask From TeamLeader
        [HttpPost]

        public IActionResult DeleteUsersTask(AddUserTaskModel addUser)
        {
            bool res;
            addUserTaskModel = new AddUserTaskModel();
            res = addUserTaskModel.delete(addUser);
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
        public IActionResult DeleteUsersTask(string id)
        {
            AddUserTaskModel addUserTaskmodel = addUserTaskModel.getData(id);
            return View(addUserTaskmodel);
        }

        public IActionResult TSetting()
        {
            return View();
        }

        public IActionResult TReport()
        {
            return View();
        }

    }

}
