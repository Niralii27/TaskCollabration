using TaskCollabration.Models;
using Microsoft.AspNetCore.Mvc;

namespace TaskCollabration.Controllers
{
    public class AdminController : Controller
    {
        AdminModel adminModel = new AdminModel();
        AdminTeamModel teamModel = new AdminTeamModel();

        public IActionResult Dashboard()
        {
            return View();
        }

       

        public IActionResult Report()
        {
            return View();
        }

        public IActionResult Project()
        {
            return View();
        }

        public IActionResult Task()
        {
            return View();
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
