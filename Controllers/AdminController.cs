using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Test.Models;
using Test.ViewModels;
namespace Test.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        public AdminController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Index(string? search)
        {
            var AdminList =await userManager.GetUsersInRoleAsync("Admin");
            TempData["search for specific Admin"] = search?.ToUpper(); // Store the uppercased AdminName in a local variable
            ViewData["checking"] = false;


            if (!string.IsNullOrEmpty(search))
            {
                AdminList = AdminList.Where(u => u.UserName.StartsWith(search)).ToList();
                if (AdminList == null)
                    {
                        ViewData["checking"] = true;
                    AdminList =await userManager.GetUsersInRoleAsync("Admin");
                    }
            }
                
                
            else
            {
                AdminList = await userManager.GetUsersInRoleAsync("Admin");
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = AdminList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(AdminList.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Search(string search)
        {

            var AdminList = await userManager.GetUsersInRoleAsync("Admin");
            TempData["search for specific Admin"] = search?.ToUpper(); // Store the uppercased AdminName in a local variable
            ViewData["checking"] = false;


            if (!string.IsNullOrEmpty(search))
            {
                AdminList = AdminList.Where(u => u.UserName.StartsWith(search)).ToList();
                if (AdminList == null)
                {
                    ViewData["checking"] = true;
                    AdminList = await userManager.GetUsersInRoleAsync("Admin");
                }
            }


            else
            {
                AdminList = await userManager.GetUsersInRoleAsync("Admin");
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = AdminList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(AdminList.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;
            return PartialView("_SearchPartial");
        }
        //add Admin to system
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterUserViewModel UserVM)
        {
            if (ModelState.IsValid && UserVM.image != null && UserVM.image.Length > 0)
            {
                // Generate a unique filename based on the person's ID
                string fileName = UserVM.FirstName.ToString() + Path.GetExtension(UserVM.image.FileName);

                // Set the image path as a combination of a directory and the filename
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Admin", fileName);

                // Save the image to the specified path
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await UserVM.image.CopyToAsync(stream);
                }

                // Update the Admin's ImagePath property
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
                    IdentityResult result2 = await userManager.AddToRoleAsync(user, "Admin");
                   //not create cookie
                   if(result2.Succeeded)
                    {
                        return RedirectToAction("index","Admin");

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
            return View(UserVM);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            // Retrieve the Admin entity from your data store based on the provided ID
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Or handle the case when the user is not found
            }
            RegisterUserViewModelForEdit uservm = new RegisterUserViewModelForEdit();
            uservm.UserId = user.Id; //it is important line
            uservm.UserName = user.UserName;
            uservm.FirstName = user.FirstName;
            uservm.LastName = user.LastName;
            uservm.Address = user.Address;

            return View(uservm);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RegisterUserViewModelForEdit UserVM, string id)
        {
            if (ModelState.IsValid && UserVM != null)
            {
                // Retrieve the existing Admin entity from your data store based on the Admin's ID
                ApplicationUser existinguser = await userManager.FindByIdAsync(id);

                if (existinguser == null)
                {
                    return NotFound(); // Or handle the case when the Admin is not found
                }
                if (UserVM.image != null && UserVM.image.Length > 0)
                {
                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(existinguser.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Admin",existinguser.ImageUrl);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Generate a unique filename based on the Admin's ID
                    string fileName = UserVM.UserName.ToString() + Path.GetExtension(UserVM.image.FileName);

                    // Set the image path as a combination of a directory and the filename
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Admin", fileName);

                    // Save the new image to the specified path
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await UserVM.image.CopyToAsync(stream);
                    }

                    // Update the Admin's ImageUrl property
                    UserVM.ImageUrl = fileName;
                    existinguser.ImageUrl = UserVM.ImageUrl;
                }

                // Update other properties of the existing Admin with the new values
                existinguser.UserName = UserVM.UserName;
                existinguser.FirstName = UserVM.FirstName;
                existinguser.LastName = UserVM.LastName;
                existinguser.PasswordHash = UserVM.Password;
                
                // Update the existing Admin entity in your data store
                IdentityResult result= await userManager.UpdateAsync(existinguser);
                if (result.Succeeded)
                {
                    // Redirect to the index action for further modifications
                    return RedirectToAction("index", "admin");

                }
                else
                {
                    foreach (var erroritem in result.Errors)
                    {
                        ModelState.AddModelError("", erroritem.Description);
                    }
                }
                
            }

            // If no image was selected or other validation failed, return to the view with the Admin model
            return View(UserVM);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            // Retrieve the existing Admin entity from your data store based on the Admin's ID
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Or handle the case when the Admin is not found
            }
            RegisterUserViewModel uservm= new RegisterUserViewModel();
            uservm.UserId = user.Id;
            uservm.UserName = user.UserName;
            uservm.FirstName = user.FirstName;
            uservm.LastName = user.LastName;
            uservm.Address = user.Address;
            uservm.ImageUrl = user.ImageUrl;
            return View(uservm);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            // Retrieve the existing Admin entity from your data store based on the Admin's ID
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Or handle the case when the Admin is not found
            }
            RegisterUserViewModel uservm = new RegisterUserViewModel();
            uservm.UserId = user.Id;
            uservm.UserName = user.UserName;
            uservm.FirstName = user.FirstName;
            uservm.LastName = user.LastName;
            uservm.Address = user.Address;
            uservm.ImageUrl = user.ImageUrl;
            uservm.ImageUrl = user.ImageUrl;
            return View(uservm);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Mvc.ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            // Retrieve the existing Admin entity from your data store based on the Admin's ID
            ApplicationUser user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(); // Or handle the case when the Admin is not found
            }

            // Delete the image if it exists
            if (!string.IsNullOrEmpty(user.ImageUrl))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Admin", user.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            // Remove the Admin entity from the data store
            IdentityResult result = await userManager.RemoveFromRoleAsync(user,"Admin");
            if (result.Succeeded)
            {

                return RedirectToAction("index", "Admin");
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
