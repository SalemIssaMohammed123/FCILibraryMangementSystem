using Microsoft.AspNetCore.Mvc;
using System.Web.Helpers;
using System.Web.Mvc;
using Test.Models;
using Test.Repositories.Author;
namespace Test.Controllers
{
    public class AuthorController : Microsoft.AspNetCore.Mvc.Controller
    {
        IAuthorRepository AuthRepo; //= new Context();
        public AuthorController(IAuthorRepository AuthRepo)
        { 
           this.AuthRepo = AuthRepo;
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Index(string? search, string? SortOrder)
        {
            var Authorlist = AuthRepo.GetAll();
            TempData["search for specific Author"] = search?.ToUpper(); // Store the uppercased AuthorName in a local variable
            TempData["sort for search for specific Author"] = SortOrder;
            ViewData["checking"] = false;


            if (!string.IsNullOrEmpty(search))
            {
                Authorlist = AuthRepo.GetAllUsingSearchWord(search);
                if (Authorlist != null)
                {

                    if (!string.IsNullOrEmpty(SortOrder))
                    {
                        switch (SortOrder)
                        {
                            case "Auth-desc":
                                Authorlist = AuthRepo.GetAllUsingSearchWordAndOrderingWithAuthorName(search);
                                break;
                            case "Desc-desc":
                                Authorlist = AuthRepo.GetAllUsingSearchWordAndOrderingWithAuthorDescription(search);
                                break;
                            case "image":
                                Authorlist = AuthRepo.GetAllUsingSearchWordAndOrderingWithAuthorImageName(search);
                                break;
                            default:
                                Authorlist = AuthRepo.GetAllUsingSearchWord(search);
                                break;
                        }
                    }
                }
                else
                {
                    ViewData["checking"] = true;
                    Authorlist = AuthRepo.GetAll();
                }
            }
            else
            {
                Authorlist = AuthRepo.GetAll();
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = Authorlist.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(Authorlist.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public Microsoft.AspNetCore.Mvc.IActionResult Search(string search, string SortOrder)
        {
            var Authorlist = AuthRepo.GetAll();
            TempData["search for specific Author"] = search?.ToUpper(); // Store the uppercased AuthorName in a local variable
            TempData["sort for search for specific Author"] = SortOrder;
            ViewData["checking"] = false;


            if (!string.IsNullOrEmpty(search))
            {
                Authorlist = AuthRepo.GetAllUsingSearchWord(search);
                if (Authorlist != null)
                {

                    if (!string.IsNullOrEmpty(SortOrder))
                    {
                        switch (SortOrder)
                        {
                            case "Auth-desc":
                                Authorlist = AuthRepo.GetAllUsingSearchWordAndOrderingWithAuthorName(search);
                                break;
                            case "Desc-desc":
                                Authorlist = AuthRepo.GetAllUsingSearchWordAndOrderingWithAuthorDescription(search);
                                break;
                            case "image":
                                Authorlist = AuthRepo.GetAllUsingSearchWordAndOrderingWithAuthorImageName(search);
                                break;
                            default:
                                Authorlist = AuthRepo.GetAllUsingSearchWord(search);
                                break;
                        }
                    }
                }
                else
                {
                    ViewData["checking"] = true;
                    Authorlist = AuthRepo.GetAll();
                }
            }
            else
            {
                Authorlist = AuthRepo.GetAll();
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = Authorlist.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(Authorlist.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;
            return PartialView("_SearchPartial") ;
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public Microsoft.AspNetCore.Mvc.JsonResult CheckAuthorNameUnique(string AuthorName, int AuthorID)
        {
            //for editting 
            bool isAuthorNameUnique = AuthRepo.CheckAuthorNameUniqueForEdit(AuthorName, AuthorID);// Logic to check if the AuthorName is unique in the database
            //for new
            if (AuthorID == 0)
                isAuthorNameUnique = AuthRepo.CheckAuthorNameUniqueForCreate(AuthorName);
            if (isAuthorNameUnique == true)
                return Json(true);
            return Json(false);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        //if new Author the id sent with 0
        public Microsoft.AspNetCore.Mvc.JsonResult CheckAuthorEmailUnique(string Email, int AuthorID)
        {
            //for editting 
            bool isAuthorEmailUnique = AuthRepo.CheckAuthorEmailUniqueForEdit(Email, AuthorID); ;
            //for new
            if (AuthorID == 0)
                isAuthorEmailUnique = AuthRepo.CheckAuthorEmailUniqueForCreate(Email);
            if (!isAuthorEmailUnique)
                return Json(true);
            return Json(false);
        }

        //add Author to system
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Create()
        {
            return View(new Author());
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Author author, IFormFile image)
        {
            if (ModelState.IsValid&& image != null && image.Length > 0)
            {

                // Generate a unique filename based on the person's ID
                string fileName = author.AuthorName.ToString() + Path.GetExtension(image.FileName);

                // Set the image path as a combination of a directory and the filename
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Author", fileName);

                // Save the image to the specified path
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Update the Author's ImagePath property
                author.ImageUrl = fileName;

                // Save the Author entity to your data store
                AuthRepo.Insert(author);


                // Redirect to the index action for further modifications
                return RedirectToAction("index");
            }
            // If no image was selected, return to the view with an error message
            //ModelState.AddModelError("", "Please select an image.");

            else
            {
                return View(author);
            }
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Edit(int id)
        {
            // Retrieve the Author entity from your data store based on the provided ID
            Author author = AuthRepo.GetById(id);

            if (author == null)
            {
                return NotFound(); // Or handle the case when the Author is not found
            }

            return View(author);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Author author,int id, IFormFile image)
        {
            if (ModelState.IsValid && author != null)
            {
                // Retrieve the existing Author entity from your data store based on the Author's ID
                Author existingAuthor = AuthRepo.GetById(id);

                if (existingAuthor == null)
                {
                    return NotFound(); // Or handle the case when the Author is not found
                }

                if (image != null && image.Length > 0)
                {
                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(existingAuthor.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Author", existingAuthor.ImageUrl);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Generate a unique filename based on the Author's ID
                    string fileName = author.AuthorName.ToString() + Path.GetExtension(image.FileName);

                    // Set the image path as a combination of a directory and the filename
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Author", fileName);

                    // Save the new image to the specified path
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Update the Author's ImageUrl property
                    author.ImageUrl = fileName;
                }

                // Update other properties of the existing Author with the new values
                existingAuthor.AuthorName = author.AuthorName;
                existingAuthor.DescripTion = author.DescripTion;
                existingAuthor.Email = author.Email;
                existingAuthor.ImageUrl = author.ImageUrl;
                // Update the existing Author entity in your data store
                AuthRepo.Update(existingAuthor);

                // Redirect to the index action for further modifications
                return RedirectToAction("Index");
            }

            // If no image was selected or other validation failed, return to the view with the Author model
            return View(author);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Details(int id)
        {
            // Retrieve the Author entity from your data store based on the provided ID
            Author author = AuthRepo.GetById(id);

            if (author == null)
            {
                return NotFound(); // Or handle the case when the Author is not found
            }

            return View(author);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Delete(int id)
        {
            // Retrieve the Author entity from your data store based on the provided ID
            Author author = AuthRepo.GetById(id);

            if (author == null)
            {
                return NotFound(); // Or handle the case when the Author is not found
            }

            return View(author);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Mvc.ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Retrieve the Author entity from your data store based on the provided ID
            Author author = AuthRepo.GetById(id);
         
            if (author == null)
            {
                return NotFound(); // Or handle the case when the Author is not found
            }

            // Delete the image if it exists
            if (!string.IsNullOrEmpty(author.ImageUrl))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Author", author.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Remove the Author entity from the data store
            AuthRepo.Delete(author);
            return RedirectToAction("Index");
        }


    }
}
