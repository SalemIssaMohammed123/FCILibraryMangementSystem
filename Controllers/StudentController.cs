using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Test.Models;
using Test.ViewModels;

namespace Test.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        public StudentController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            var StudentList = await userManager.GetUsersInRoleAsync("Student");
            TempData["search for specific Student"] = search?.ToUpper(); // Store the uppercased StudentName in a local variable
            ViewData["checking"] = false;


            if (!string.IsNullOrEmpty(search))
            {
                StudentList = (IList<ApplicationUser>)StudentList.Where(u => u.UserName.StartsWith(search));
                if (StudentList == null)
                {
                    ViewData["checking"] = true;
                    StudentList = await userManager.GetUsersInRoleAsync("Student");
                }
            }


            else
            {
                StudentList = await userManager.GetUsersInRoleAsync("Student");
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = StudentList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(StudentList.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Search(string search)
        {

            var StudentList = await userManager.GetUsersInRoleAsync("Student");
            TempData["search for specific Student"] = search?.ToUpper(); // Store the uppercased StudentName in a local variable
            ViewData["checking"] = false;


            if (!string.IsNullOrEmpty(search))
            {
                StudentList = (IList<ApplicationUser>)StudentList.Where(u => u.UserName.StartsWith(search));
                if (StudentList == null)
                {
                    ViewData["checking"] = true;
                    StudentList = await userManager.GetUsersInRoleAsync("Student");
                }
            }


            else
            {
                StudentList = await userManager.GetUsersInRoleAsync("Student");
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = StudentList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(StudentList.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;
            return PartialView("_SearchPartial");
        }
        //add Student to system
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterUserViewModel UserVM, IFormFile image)
        {
            if (ModelState.IsValid && image != null && image.Length > 0)
            {
                // Generate a unique filename based on the Student's UserName
                string fileName = UserVM.FirstName.ToString() + Path.GetExtension(image.FileName);

                // Set the image path as a combination of a directory and the filename
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Student", fileName);

                // Save the image to the specified path
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Update the Author's ImagePath property
                UserVM.ImageUrl = fileName;
                //Mapping from ViewModel to Model
                ApplicationUser user = new ApplicationUser();
                user.UserName = UserVM.UserName;
                user.FirstName = UserVM.FirstName;
                user.LastName = UserVM.LastName;
                user.ImageUrl = UserVM.ImageUrl;
                user.PasswordHash = UserVM.Password;
                user.Address = UserVM.Address;
                IdentityResult result1 = await userManager.CreateAsync(user, UserVM.Password);
                if (result1.Succeeded)
                {
                    IdentityResult result2 = await userManager.AddToRoleAsync(user, "Student");
                    //not create cookie
                    if (result2.Succeeded)
                    {
                        return RedirectToAction("Student", "index");

                    }
                    else
                    {
                        foreach (var erroritem in result2.Errors)
                        {
                            ModelState.AddModelError("", erroritem.Description);
                        }
                    }
                }
                else
                {
                    foreach (var erroritem in result1.Errors)
                    {
                        ModelState.AddModelError("Password", erroritem.Description);
                    }
                }
            }
            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            // Retrieve the Student entity from your data store based on the provided ID
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Or handle the case when the user is not found
            }
            RegisterUserViewModel uservm = new RegisterUserViewModel();
            uservm.UserId = user.Id; //it is important line
            uservm.UserName = user.UserName;
            uservm.FirstName = user.FirstName;
            uservm.LastName = user.LastName;
            uservm.Address = user.Address;

            return View(uservm);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterUserViewModel UserVM, string id, IFormFile image)
        {
            if (ModelState.IsValid && UserVM != null)
            {
                // Retrieve the existing Student entity from your data store based on the Student's ID
                ApplicationUser existinguser = await userManager.FindByIdAsync(id);

                if (existinguser == null)
                {
                    return NotFound(); // Or handle the case when the Student is not found
                }
                if (image != null && image.Length > 0)
                {
                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(existinguser.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Student", existinguser.ImageUrl);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Generate a unique filename based on the Student's ID
                    string fileName = UserVM.UserName.ToString() + Path.GetExtension(image.FileName);

                    // Set the image path as a combination of a directory and the filename
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Student", fileName);

                    // Save the new image to the specified path
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Update the Student's ImageUrl property
                    UserVM.ImageUrl = fileName;
                }

                // Update other properties of the existing Student with the new values
                existinguser.UserName = UserVM.UserName;
                existinguser.FirstName = UserVM.FirstName;
                existinguser.LastName = UserVM.LastName;
                existinguser.PasswordHash = UserVM.Password;
                existinguser.ImageUrl = UserVM.ImageUrl;

                // Update the existing Student entity in your data store
                IdentityResult result = await userManager.UpdateAsync(existinguser);
                if (result.Succeeded)
                {
                    // Redirect to the index action for further modifications
                    return RedirectToAction("Student", "index");

                }
                else
                {
                    foreach (var erroritem in result.Errors)
                    {
                        ModelState.AddModelError("", erroritem.Description);
                    }
                }

            }

            // If no image was selected or other validation failed, return to the view with the Student model
            return View(UserVM);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            // Retrieve the existing Student entity from your data store based on the Student's ID
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Or handle the case when the Student is not found
            }
            RegisterUserViewModel uservm = new RegisterUserViewModel();
            uservm.UserId = user.Id;
            uservm.UserName = user.UserName;
            uservm.FirstName = user.FirstName;
            uservm.LastName = user.LastName;
            uservm.Address = user.Address;

            return View(uservm);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            // Retrieve the existing Student entity from your data store based on the Student's ID
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Or handle the case when the Student is not found
            }
            RegisterUserViewModel uservm = new RegisterUserViewModel();
            uservm.UserId = user.Id;
            uservm.UserName = user.UserName;
            uservm.FirstName = user.FirstName;
            uservm.LastName = user.LastName;
            uservm.Address = user.Address;

            return View(uservm);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Mvc.ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Retrieve the existing Student entity from your data store based on the Student's ID
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Or handle the case when the Student is not found
            }

            // Delete the image if it exists
            if (!string.IsNullOrEmpty(user.ImageUrl))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Student", user.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Remove the Student entity from the data store
            IdentityResult result = await userManager.RemoveFromRoleAsync(user, "Student");
            if (result.Succeeded)
            {

                return RedirectToAction("Student", "index");
            }
            else
            {
                foreach (var erroritem in result.Errors)
                {
                    ModelState.AddModelError("", erroritem.Description);
                }
            }
            return View("Errors");
        }
    }
}
