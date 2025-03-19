using TaskCollabration.Models;
using Microsoft.AspNetCore.Mvc;

namespace TaskCollabration.Controllers
{
    public class AdminController : Controller
    {
        AdminModel adminModel = new AdminModel();

        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Team()
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
            List<AdminModel> adminModels= adminModel.getData();
            return View(adminModels);
        }

        [HttpPost]

        public IActionResult EditUser(AdminModel adminModel1)
        {
            bool res;
            if (!ModelState.IsValid)
            {
                adminModel= new AdminModel();
                res =  adminModel.update(adminModel1);
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

        public IActionResult EditAdminProfile()
        {
            return View();
        }
    }
}
