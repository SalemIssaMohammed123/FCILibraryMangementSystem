using Microsoft.AspNetCore.Mvc;
using Test.Models;
using Test.Repositories.Publisher;
using Test.ViewModels;
namespace Test.Controllers
{
    public class PublisherController : Controller
    {
        IPublisherRepository PublishRepo;// = new Context();
        public PublisherController(IPublisherRepository PublishRepo) 
        {
            this.PublishRepo = PublishRepo;
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Index(string? search, string? SortOrder)
        {
            var Publisherlist = PublishRepo.GetAll();
            TempData["search for specific publisher"] = search?.ToUpper(); // Store the uppercased PublisherName in a local variable
            TempData["sort for search for specific publisher"] = SortOrder;
            ViewData["checking"] = false;


            if (!string.IsNullOrEmpty(search))
            {
                Publisherlist = PublishRepo.GetAllUsingSearchWord(search);
                if (Publisherlist != null)
                {

                    if (!string.IsNullOrEmpty(SortOrder))
                    {
                        switch (SortOrder)
                        {
                            case "Publish-desc":
                                Publisherlist = PublishRepo.GetAllUsingSearchWordAndOrderingWithPublisherName(search);
                                break;
                            case "Desc-desc":
                                Publisherlist = PublishRepo.GetAllUsingSearchWordAndOrderingWithPublisherDescription(search);
                                break;
                            case "image":
                                Publisherlist = PublishRepo.GetAllUsingSearchWordAndOrderingWithPublisherImageName(search);
                                break;
                            default:
                                Publisherlist = PublishRepo.GetAllUsingSearchWord(search);
                                break;
                        }
                    }
                }
                else
                {
                    ViewData["checking"] = true;
                    Publisherlist = PublishRepo.GetAll();
                }
            }
            else
            {
                Publisherlist = PublishRepo.GetAll();
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = Publisherlist.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(Publisherlist.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public Microsoft.AspNetCore.Mvc.IActionResult Search(string search, string SortOrder)
        {
            var Publisherlist = PublishRepo.GetAll();
            TempData["search for specific publisher"] = search?.ToUpper(); // Store the uppercased PublisherName in a local variable
            TempData["sort for search for specific publisher"] = SortOrder;
            ViewData["checking"] = false;


            if (!string.IsNullOrEmpty(search))
            {
                Publisherlist = PublishRepo.GetAllUsingSearchWord(search);
                if (Publisherlist != null)
                {

                    if (!string.IsNullOrEmpty(SortOrder))
                    {
                        switch (SortOrder)
                        {
                            case "Publish-desc":
                                Publisherlist = PublishRepo.GetAllUsingSearchWordAndOrderingWithPublisherName(search);
                                break;
                            case "Desc-desc":
                                Publisherlist = PublishRepo.GetAllUsingSearchWordAndOrderingWithPublisherDescription(search);
                                break;
                            case "image":
                                Publisherlist = PublishRepo.GetAllUsingSearchWordAndOrderingWithPublisherImageName(search);
                                break;
                            default:
                                Publisherlist = PublishRepo.GetAllUsingSearchWord(search);
                                break;
                        }
                    }
                }
                else
                {
                    ViewData["checking"] = true;
                    Publisherlist = PublishRepo.GetAll();
                }
            }
            else
            {
                Publisherlist = PublishRepo.GetAll();
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = Publisherlist.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(Publisherlist.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;
            return PartialView("_SearchPartial");
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public Microsoft.AspNetCore.Mvc.JsonResult CheckPublisherNameUnique(string PublisherName)
        {
           
            bool   isPublisherNameUnique = PublishRepo.CheckPublisherNameUniqueForCreate(PublisherName);
            if (isPublisherNameUnique == true)
                return Json(true);
            return Json(false);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public Microsoft.AspNetCore.Mvc.JsonResult CheckPublisherEmailUnique(string Email)
        {
            
             bool isPublisherEmailUnique = PublishRepo.CheckPublisherEmailUniqueForCreate(Email);
            if (isPublisherEmailUnique)
                return Json(true);
            return Json(false);
        }

        //add publisher to system
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Create()
        {
            return View(new PublisherViewModel());
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PublisherViewModel publisherVM)
        {
            if (ModelState.IsValid && publisherVM.image != null && publisherVM.image.Length > 0)
            {

                // Generate a unique filename based on the person's ID
                string fileName = publisherVM.PublisherName.ToString() + Path.GetExtension(publisherVM.image.FileName);

                // Set the image path as a combination of a directory and the filename
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Publisher", fileName);

                // Save the image to the specified path
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await publisherVM.image.CopyToAsync(stream);
                }

                publisherVM.ImgUrl = fileName;
                Publisher publisher = new Publisher();
                publisher.PublisherName = publisherVM.PublisherName;
                publisher.Description = publisherVM.Description;
                publisher.Email = publisherVM.Email;
                // Update the Publisher's ImagePath property
                publisher.ImageUrl = fileName;
                // Save the Publisher entity to your data store
                PublishRepo.Insert(publisher);


                // Redirect to the index action for further modifications
                return RedirectToAction("index");
            }
            // If no image was selected, return to the view with an error message
            //ModelState.AddModelError("", "Please select an image.");

            else
            {
                return View(publisherVM);
            }
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Edit(int id)
        {
            // Retrieve the Publisher entity from your data store based on the provided ID
            Publisher publisher = PublishRepo.GetById(id);

            if (publisher == null)
            {
                return NotFound(); // Or handle the case when the publisher is not found
            }
            PublisherViewModelForEdit publisherVM= new PublisherViewModelForEdit();
            publisherVM.PublisherID = publisher.PublisherID;
            publisherVM.PublisherName = publisher.PublisherName;
            publisherVM.Description = publisher.Description;
            publisherVM.Email = publisher.Email;
            publisherVM.ImgUrl = publisher.ImageUrl;
            return View(publisherVM);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PublisherViewModelForEdit publisherVM,int id)
        {
            string newFileName = null;
            if (ModelState.IsValid && publisherVM != null)
            {
                // Retrieve the existing Publisher entity from your data store based on the Publisher's ID
                Publisher existingPublisher = PublishRepo.GetById(id);

                if (existingPublisher == null)
                {
                    return NotFound(); // Or handle the case when the Publisher is not found
                }

                if (publisherVM.image != null && publisherVM.image.Length > 0)
                {
                    // Delete the old image if it exists
                    if (!string.IsNullOrEmpty(existingPublisher.ImageUrl))
                    {
                        string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Publisher", existingPublisher.ImageUrl);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Generate a unique filename based on the Publisher's ID
                    string fileName = publisherVM.PublisherName.ToString() + Path.GetExtension(publisherVM.image.FileName);

                    // Set the image path as a combination of a directory and the filename
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Publisher", fileName);

                    // Save the new image to the specified path
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await publisherVM.image.CopyToAsync(stream);
                    }

                    // Update the Publisher's ImageUrl property
                    newFileName = fileName;
                    existingPublisher.ImageUrl = newFileName;

                }

                // Update other properties of the existing Publisher with the new values
                existingPublisher.PublisherName = publisherVM.PublisherName;
                existingPublisher.Description = publisherVM.Description;
                existingPublisher.Email = publisherVM.Email;
                // Update the existing Publisher entity in your data store
               PublishRepo.Update(existingPublisher);

                // Redirect to the index action for further modifications
                return RedirectToAction("Index");
            }

            // If no image was selected or other validation failed, return to the view with the publisher model
            return View(publisherVM);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Details(int id)
        {
            // Retrieve the Publisher entity from your data store based on the provided ID
            Publisher publisher = PublishRepo.GetById(id);

            if (publisher == null)
            {
                return NotFound(); // Or handle the case when the publisher is not found
            }

            PublisherViewModel publisherVM = new PublisherViewModel();
            publisherVM.PublisherID = publisher.PublisherID;
            publisherVM.PublisherName = publisher.PublisherName;
            publisherVM.Description = publisher.Description;
            publisherVM.Email = publisher.Email;
            publisherVM.ImgUrl = publisher.ImageUrl;
            return View(publisher);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Delete(int id)
        {
            // Retrieve the Publisher entity from your data store based on the provided ID
            Publisher publisher = PublishRepo.GetById(id);

            if (publisher == null)
            {
                return NotFound(); // Or handle the case when the publisher is not found
            }

            PublisherViewModel publisherVM = new PublisherViewModel();
            publisherVM.PublisherID = publisher.PublisherID;
            publisherVM.PublisherName = publisher.PublisherName;
            publisherVM.Description = publisher.Description;
            publisherVM.Email = publisher.Email;
            publisherVM.ImgUrl = publisher.ImageUrl;
            return View(publisher);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Mvc.ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Retrieve the Publisher entity from your data store based on the provided ID
            Publisher publisher = PublishRepo.GetById(id);

            if (publisher == null)
            {
                return NotFound(); // Or handle the case when the publisher is not found
            }
            PublisherViewModel publisherVM = new PublisherViewModel();
            publisherVM.PublisherID = publisher.PublisherID;
            publisherVM.PublisherName = publisher.PublisherName;
            publisherVM.Description = publisher.Description;
            publisherVM.Email = publisher.Email;
            publisherVM.ImgUrl = publisher.ImageUrl;
            // Delete the image if it exists
            if (!string.IsNullOrEmpty(publisher.ImageUrl))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Publisher", publisherVM.ImgUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Remove the Publisher entity from the data store
            PublishRepo.Delete(publisher);
            return RedirectToAction("Index");
        }
    }
}
