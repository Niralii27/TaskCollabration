using TaskCollabration.Models;
using Microsoft.AspNetCore.Mvc;

namespace TaskCollabration.Controllers
{
    public class AdminController : Controller
    {
        AdminModel adminModel = new AdminModel();
        AdminTeamModel teamModel = new AdminTeamModel();
        AdminProjectModel projectModel = new AdminProjectModel();
        AdminTaskModel taskModel = new AdminTaskModel();


        public IActionResult Dashboard()
        {
            AdminProjectModel projectModel = new AdminProjectModel();

            // Fetch all projects safely
            List<AdminProjectModel> projects = projectModel.getData() ?? new List<AdminProjectModel>();

            // Take only the last 3 projects (assuming they are ordered by ID or Date)
            projects = projects.OrderByDescending(p => p.Id).Take(3).ToList();

            return View(projects);
        }



        public IActionResult Report()
        {
            return View();
        }

        public IActionResult Project()
        {
            List<AdminProjectModel> projects = projectModel.getData();
            return View(projects);
        }

        public IActionResult AddProject()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProject(AdminProjectModel project)
        {
            if (ModelState.IsValid)
            {
                bool result = projectModel.insert(project);
                if (result)
                {
                    TempData["msg"] = "Project added successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add project.";
                }
                return RedirectToAction("Project", "Admin"); // Redirect to Project Page
            }

            return View(project);
        }

        [HttpGet]
        public IActionResult EditProject(string id)
        {
            AdminProjectModel project = projectModel.getData(id);
            return View(project);
        }

        [HttpPost]
        public IActionResult EditProject(AdminProjectModel project)
        {
            if (ModelState.IsValid)
            {
                bool result = projectModel.update(project);
                if (result)
                {
                    TempData["msg"] = "Project updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update project.";
                }
                return RedirectToAction("Project", "Admin"); // Redirect to Project Page
            }

            TempData["ErrorMessage"] = "Validation error!";
            return View(project);
        }

        [HttpGet]
        public IActionResult DeleteProject(string id)
        {
            AdminProjectModel project = projectModel.getData(id);
            return View(project);
        }

        [HttpPost]
        public IActionResult DeleteProject(AdminProjectModel project)
        {
            bool result = projectModel.delete(project);
            if (result)
            {
                TempData["msg"] = "Project deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete project.";
            }

            return RedirectToAction("Project", "Admin"); // Redirect to Project Page
        }


        // List all tasks
        public IActionResult Task()
        {
            List<AdminTaskModel> tasks = taskModel.GetData();
            return View(tasks);
        }

        // Add Task (GET)
        public IActionResult AddTask()
        {
            return View();
        }

        // Add Task (POST)
        [HttpPost]
        public IActionResult AddTask(AdminTaskModel task)
        {
            if (ModelState.IsValid)
            {
                bool result = taskModel.Insert(task);
                if (result)
                {
                    TempData["msg"] = "Task added successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to add task.";
                }
                return RedirectToAction("Task");
            }

            return View(task);
        }

        // Edit Task (GET)
        [HttpGet]
        public IActionResult EditTask(int id)
        {
            AdminTaskModel task = taskModel.GetData(id);
            return View(task);
        }

        // Edit Task (POST)
        [HttpPost]
        public IActionResult EditTask(AdminTaskModel task)
        {
            if (ModelState.IsValid)
            {
                bool result = taskModel.Update(task);
                if (result)
                {
                    TempData["msg"] = "Task updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update task.";
                }
                return RedirectToAction("Task");
            }
            TempData["ErrorMessage"] = "Validation error!";
            return View(task);
        }

        // Delete Task (GET)
        [HttpGet]
        public IActionResult DeleteTask(int id)
        {
            AdminTaskModel task = taskModel.GetData(id);
            return View(task);
        }

        // Delete Task (POST)
        [HttpPost]
        public IActionResult DeleteTask(AdminTaskModel task)
        {
            bool result = taskModel.Delete(task.Id);
            if (result)
            {
                TempData["msg"] = "Task deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete task.";
            }
            return RedirectToAction("Task");
        }



        public IActionResult AddUser()
        {
            return View();
        }



        [HttpPost]
        public IActionResult AddUser(AdminModel admin, IFormFile imageFile)
        {
            // Handle file upload
            if (imageFile != null && imageFile.Length > 0)
            {
                // Use original filename without any random prefix
                string fileName = imageFile.FileName;

                // Set path to wwwroot/Images folder
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");

                // Create directory if it doesn't exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string filePath = Path.Combine(uploadsFolder, fileName);

                // Save the file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                // Set the image path in your model
                admin.Image = "" + fileName;
            }

            // Initialize model and insert data
            adminModel = new AdminModel();
            bool result = adminModel.insert(admin);

            if (result)
            {
                TempData["msg"] = "Added Successfully";
            }
            else
            {
                TempData["msg"] = "Failed To Add Data";
            }

            return View();
        }
        public IActionResult ListUser()
        {
            adminModel = new AdminModel();
            List<AdminModel> adminModels = adminModel.getData();
            return View(adminModels);
        }

        [HttpPost]

        public IActionResult EditUser(AdminModel adminModel1)
        {
            bool res;
            if (!ModelState.IsValid)
            {
                adminModel = new AdminModel();
                res = adminModel.update(adminModel1);
                if (res)
                {
                    TempData["msg"] = "Updated Successfully";
                }
                else
                {
                    TempData["msg"] = "Failed to update";
                }
            }
            return View();

        }

        [HttpGet]
        public IActionResult EditUser(string id)
        {
            AdminModel admin = adminModel.getData(id);
            return View(admin);
        }

        [HttpPost]

        public IActionResult DeleteUser(AdminModel model)
        {
            bool res;

            adminModel = new AdminModel();
            res = adminModel.delete(model);
            if (res)
            {
                TempData["msg"] = "Deleted successfully";

            }
            else
            {
                TempData["msg"] = "Not Deleted. something went wrong..!!";

            }
            return View();
        }


        [HttpGet]
        public IActionResult DeleteUser(string id)
        {
            AdminModel adminmodel = adminModel.getData(id);
            return View(adminmodel);
        }
        public IActionResult EditAdminProfile()
        {
            return View();
        }



        //admin team
        //for Team
        public IActionResult Team()
        {
            List<AdminTeamModel> teams = teamModel.getData();
            return View(teams);
        }

        public IActionResult AddTeam()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddTeam(AdminTeamModel team, IFormFile Attachments)
        {
            if (Attachments != null && Attachments.Length > 0)
            {
                var fileName = Path.GetFileName(Attachments.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Attachments.CopyTo(stream);
                }
                team.Attachments = fileName; // Save filename to database
            }

            if (ModelState.IsValid)
            {
                bool result = teamModel.insert(team);
                TempData["msg"] = result ? "Team task added successfully!" : "Failed to add team task.";
                return RedirectToAction("Team", "Admin");
            }
            return View(team);
        }

        [HttpGet]
        public IActionResult EditTeam(string id)
        {
            AdminTeamModel team = teamModel.getData(id);
            return View(team);
        }


        [HttpPost]
        public IActionResult EditTeam(AdminTeamModel team, IFormFile Attachments)
        {
            if (Attachments != null && Attachments.Length > 0)
            {
                var fileName = Path.GetFileName(Attachments.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Attachments.CopyTo(stream);
                }

                team.Attachments = fileName; // Save the filename to the database
            }

            if (ModelState.IsValid)
            {
                bool result = teamModel.update(team);
                if (result)
                {
                    TempData["msg"] = "Team task updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update team task.";
                }
                return RedirectToAction("Team", "Admin");
            }

            TempData["ErrorMessage"] = "Validation error!";
            return View(team);
        }


        [HttpGet]
        public IActionResult DeleteTeam(string id)
        {
            AdminTeamModel team = teamModel.getData(id);
            return View(team);
        }

        [HttpPost]
        public IActionResult DeleteTeam(AdminTeamModel team)
        {
            bool result = teamModel.delete(team);
            if (result)
            {
                TempData["msg"] = "Team task deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete team task.";
            }
            return RedirectToAction("Team", "Admin");
        }
    }
}