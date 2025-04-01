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
        TeamModel teamModel = new TeamModel();
        TeamLeaderProfileModel teamLeaderProfileModel= new TeamLeaderProfileModel();

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
        public IActionResult TProjectDetails(int id)
        {
            if (id <= 0)
            {
                TempData["msg"] = "Invalid Project ID.";
                return RedirectToAction("SomeErrorPage");
            }

            // Get project data
            ProjectModel project = projectmodel.getData(id.ToString());

            // Get messages for this project
            MessageModel messageModel = new MessageModel();
            List<MessageModel> projectMessages = messageModel.GetProjectMessages(id);

            // Pass the messages to the view
            ViewBag.ProjectMessages = projectMessages;
            ViewBag.ProjectId = id;

            return View(project);
        }
        [HttpPost]
        public IActionResult AddProjectMessage([Bind("Message,UserId,ProjectId")] MessageModel message, int ProjectId)
        {
            try
            {
                // Log received data
                System.Diagnostics.Debug.WriteLine($"Form data: Message='{Request.Form["Message"]}', ProjectId={ProjectId}");

                // Session se UserID fetch karna
                int? userId = HttpContext.Session.GetInt32("UserID");
                // Agar UserID session mein nahi hai, to Login page pe redirect karo
                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                // Manually set message from form
                message.Message = Request.Form["Message"].ToString();
                message.UserId = userId.Value;
                message.ProjectId = ProjectId;

                System.Diagnostics.Debug.WriteLine($"After binding: Message='{message.Message}', ProjectId={message.ProjectId}, UserId={message.UserId}");

                // Check if message is empty
                if (string.IsNullOrEmpty(message.Message))
                {
                    TempData["msg"] = "Message cannot be empty.";
                    return RedirectToAction("TProjectDetails", new { id = message.ProjectId });
                }

                message.DateTime = DateTime.Now; // Set current date time
                bool result = message.insert(message); // Insert message into database
                TempData["msg"] = result ? "Message added successfully!" : "Failed to add message.";
                return RedirectToAction("TProjectDetails", new { id = message.ProjectId });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                TempData["msg"] = "An error occurred: " + ex.Message;
                return RedirectToAction("TProjectDetails", new { id = ProjectId });
            }
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
            teamModel = new TeamModel();
            List<TeamModel> teamModels = teamModel.getData();
            return View(teamModels);
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

      
        [HttpGet]
        [Route("User/TSetting/{id}")]  // Specific route for ID-based retrieval
        public IActionResult SettingById(string id)
        {
            TeamLeaderProfileModel user = teamLeaderProfileModel.getData(id);
            return View(user);
        }

        [HttpGet]
        [Route("TeamLeader/TSetting")]  // Default route without ID
        public IActionResult TSetting()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.UserID = userId;
            TeamLeaderProfileModel teamLeaderProfileModel = new TeamLeaderProfileModel();
            List<TeamLeaderProfileModel> users = teamLeaderProfileModel.getdata(userId.Value);

            var viewModel = new TeamLeaderProfileModel
            {
                SettingList1 = users ?? new List<TeamLeaderProfileModel>()
            };

            return View(viewModel);
        }

        //Update a Setting Data

        [HttpPost]
        public IActionResult TSetting(TeamLeaderProfileModel model)
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
                return RedirectToAction("TSetting");
            }
            else
            {
                TempData["msg"] = "Failed to Update Data";
            }
            return View(model);
        }

        public IActionResult TReport()
        {
            return View();
        }

    }

}
