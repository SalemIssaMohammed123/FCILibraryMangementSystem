using Microsoft.AspNetCore.Mvc;
using System.Net;
using Test.Models;
using Test.Repositories.Author;
using Test.Repositories.Book;
using Test.Repositories.Departement;
using Test.Repositories.Publisher;
using Test.ViewModels;

namespace Test.Controllers
{
    public class BookController : Controller
    {
        IBookRepository BookRepo { get; }
        public IAuthorRepository AuthRepo { get; }
        public IPublisherRepository PublishRepo { get; }
        public IDepartementRepository DeptRepo { get; }

        public FCI_LibraryMangementSystemViewModel LibMangeSys= new FCI_LibraryMangementSystemViewModel();
        public BookController(IBookRepository BookRepo,IAuthorRepository AuthRepo,IPublisherRepository PublishRepo,IDepartementRepository DeptRepo)
        { 
          this.BookRepo = BookRepo;
            this.AuthRepo = AuthRepo;
            this.PublishRepo = PublishRepo;
            this.DeptRepo = DeptRepo;
        }
        public IActionResult Index(string? search, string? SortOrder)
        {
            var Booklist = BookRepo.GetAll();
            TempData["search for specific Book"] = search?.ToUpper(); // Store the uppercased BookName in a local variable
            TempData["sort for search for specific Book"] = SortOrder;
            ViewData["checking"] = false;


            if (!string.IsNullOrEmpty(search))
            {
                Booklist = BookRepo.GetAllUsingSearchWord(search);
                if (Booklist != null)
                {

                    if (!string.IsNullOrEmpty(SortOrder))
                    {
                        switch (SortOrder)
                        {
                            case "DeptName-desc":
                                Booklist = BookRepo.GetAllUsingSearchWordAndOrderingWithBookDepartementNameWithDescendingOrder(search);
                                break;
                            case "Book-Name":
                                Booklist = BookRepo.GetAllUsingSearchWordAndOrderingWithBookTitle(search);
                                break;
                            case "NumberOfPagesOfTheBooks":
                                Booklist = BookRepo.GetAllUsingSearchWordAndOrderingWithNumberOfPagesOfTheBooks(search);
                                break;
                            default:
                                Booklist = BookRepo.GetAllUsingSearchWord(search);
                                break;
                        }
                    }
                }
                else
                {
                    ViewData["checking"] = true;
                    Booklist = BookRepo.GetAll();
                }
            }
            else
            {
                Booklist = BookRepo.GetAll();
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = Booklist.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(Booklist.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public Microsoft.AspNetCore.Mvc.IActionResult Search(string search, string SortOrder)
        {
            var Booklist = BookRepo.GetAll();
            TempData["search for specific Book"] = search?.ToUpper(); // Store the uppercased BookName in a local variable
            TempData["sort for search for specific Book"] = SortOrder;
            ViewData["checking"] = false;


            if (!string.IsNullOrEmpty(search))
            {
                Booklist = BookRepo.GetAllUsingSearchWord(search);
                if (Booklist != null)
                {

                    if (!string.IsNullOrEmpty(SortOrder))
                    {
                        switch (SortOrder)
                        {
                            case "DeptName-desc":
                                Booklist = BookRepo.GetAllUsingSearchWordAndOrderingWithBookDepartementNameWithDescendingOrder(search);
                                break;
                            case "Book-Name":
                                Booklist = BookRepo.GetAllUsingSearchWordAndOrderingWithBookTitle(search);
                                break;
                            case "NumberOfPagesOfTheBooks":
                                Booklist = BookRepo.GetAllUsingSearchWordAndOrderingWithNumberOfPagesOfTheBooks(search);
                                break;
                            default:
                                Booklist = BookRepo.GetAllUsingSearchWord(search);
                                break;
                        }
                    }
                }
                else
                {
                    ViewData["checking"] = true;
                    Booklist = BookRepo.GetAll();
                }
            }
            else
            {
                Booklist = BookRepo.GetAll();
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = Booklist.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(Booklist.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;
            return PartialView("_SearchPartial");
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        //if new Book the id sent with 0
        public Microsoft.AspNetCore.Mvc.JsonResult Check_BookISBN_Unique(string ISBN, int BookorID)
        {
            //for editting 
            bool isBookorNameUnique = BookRepo.Check_BookISBN_UniqueForEdit( ISBN, BookorID);// Logic to check if the BookISBN is unique in the database
            //for new
            if (BookorID == 0)
                isBookorNameUnique = BookRepo.Check_BookISBN_UniqueForCreate(ISBN);
            if (isBookorNameUnique == true)
                return Json(true);
            return Json(false);
        }
        //add Bookor to system
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Create()
        {
            LibMangeSys.Authors = AuthRepo.GetAllAsList();
            LibMangeSys.Publishers = PublishRepo.GetAllAsList();
            LibMangeSys.Departements=DeptRepo.GetAllAsList();
            ViewData["FCI_LibraryMangementSystemViewModel"] = LibMangeSys;
            return View(new Book());
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book, IFormFile image)
        {
            if (ModelState.IsValid && image != null && image.Length > 0)
            {
                try
                {
                    // Generate a unique filename based on the Book's Title
                    string fileName = book.BookTitle.ToString() + Path.GetExtension(image.FileName);

                    // Set the image path as a combination of a directory and the filename
                    string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Book", fileName);

                    // Save the image to the specified path
                    using (var stream = new FileStream(imagePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                    // Update the Book's ImagePath property
                    book.ImageUrl = fileName;

                    // Save the Book entity to your data store
                    BookRepo.Insert(book);
                }
                 catch(Exception ex)
                {
                    ModelState.AddModelError("DepartmentID", "Select the departement for this book");
                    ModelState.AddModelError("AuthorID", "Select the authorName for this book");
                    ModelState.AddModelError("PublisherID", "Select the publisherName for this book");
                }
                // Redirect to the index action for further modifications
                return RedirectToAction("index");
            }
            // If no image was selected, return to the view with an error message
            //ModelState.AddModelError("", "Please select an image.");

            else
            {
                LibMangeSys.Authors = AuthRepo.GetAllAsList();
                LibMangeSys.Publishers = PublishRepo.GetAllAsList();
                LibMangeSys.Departements = DeptRepo.GetAllAsList();
                ViewData["FCI_LibraryMangementSystemViewModel"] = LibMangeSys;
                return View(book);
            }
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Edit(int id)
        {
            // Retrieve the Book entity from your data store based on the provided ID
            Book book = BookRepo.GetById(id);

            if (book == null)
            {
                return NotFound(); // Or handle the case when the Book is not found
            }

            LibMangeSys.Authors = AuthRepo.GetAllAsList();
            LibMangeSys.Publishers = PublishRepo.GetAllAsList();
            LibMangeSys.Departements = DeptRepo.GetAllAsList();
            ViewData["FCI_LibraryMangementSystemViewModel"] = LibMangeSys;
            return View(book);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book, int id, IFormFile image)
        {
            
                if (ModelState.IsValid && book != null)
                {
                   try 
                      {
                    // Retrieve the existing Book entity from your data store based on the Book's ID
                    Book existingBook = BookRepo.GetById(id);

                    if (existingBook == null)
                    {
                        return NotFound(); // Or handle the case when the Bookor is not found
                    }

                    if (image != null && image.Length > 0)
                    {
                        // Delete the old image if it exists
                        if (!string.IsNullOrEmpty(existingBook.ImageUrl))
                        {
                            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Book", existingBook.ImageUrl);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Generate a unique filename based on the Bookor's ID
                        string fileName = book.BookTitle.ToString() + Path.GetExtension(image.FileName);

                        // Set the image path as a combination of a directory and the filename
                        string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Bookor", fileName);

                        // Save the new image to the specified path
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        // Update the Bookor's ImageUrl property
                        book.ImageUrl = fileName;
                    }

                    // Update other properties of the existing Book with the new values
                    existingBook.BookTitle = book.BookTitle;
                    existingBook.ISBN = book.ISBN;
                    existingBook.Count = book.Count;
                    existingBook.NoOfPage = book.NoOfPage;
                    existingBook.ImageUrl = book.ImageUrl;
                    // Update the existing Bookor entity in your data store
                    BookRepo.Update(existingBook);
                         }
                   catch (Exception ex)
                      {
                         ModelState.AddModelError("DepartmentID", "Select the departement for this book");
                         ModelState.AddModelError("AuthorID", "Select the authorName for this book");
                         ModelState.AddModelError("PublisherID", "Select the publisherName for this book");
                      }

                // Redirect to the index action for further modifications
                return RedirectToAction("Index");
                }

            LibMangeSys.Authors = AuthRepo.GetAllAsList();
            LibMangeSys.Publishers = PublishRepo.GetAllAsList();
            LibMangeSys.Departements = DeptRepo.GetAllAsList();
            ViewData["FCI_LibraryMangementSystemViewModel"] = LibMangeSys;
            // If no image was selected or other validation failed, return to the view with the Book model
            return View(book);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Details(int id)
        {
            // Retrieve the Book entity from your data store based on the provided ID
            Book book = BookRepo.GetById(id);

            if (book == null)
            {
                return NotFound(); // Or handle the case when the Book is not found
            }

            return View(book);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Delete(int id)
        {
            // Retrieve the Book entity from your data store based on the provided ID
            Book book = BookRepo.GetById(id);

            if (book == null)
            {
                return NotFound(); // Or handle the case when the Book is not found
            }

            return View(book);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Mvc.ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Retrieve the Book entity from your data store based on the provided ID
            Book book = BookRepo.GetById(id);

            if (book == null)
            {
                return NotFound(); // Or handle the case when the Book is not found
            }
            // Delete the image if it exists
            if (!string.IsNullOrEmpty(book.ImageUrl))
            {
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/Book", book.ImageUrl);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Remove the Book entity from the data store
            BookRepo.Delete(book);
            return RedirectToAction("Index");
        }

    }
}
