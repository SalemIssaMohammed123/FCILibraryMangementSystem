using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Test.Models;
using Test.Repositories.Book;
using Test.ViewModels;
namespace Test.Controllers
{
    public class BorrowController : Controller
    {
        private readonly UserManager<ApplicationUser> userManeger;
        private readonly IBorrowRepository borrowRepo;

        public BorrowController(UserManager<ApplicationUser> userManeger,IBorrowRepository borrowRepo)
        {
            this.userManeger = userManeger;
            this.borrowRepo = borrowRepo;
        }
        //there exist unuse for using inject IBookRepository because i use it in the view directly
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Index(string? search)
        {
            var Borrowlist = borrowRepo.GetAll();
            TempData["search for specific User who borrow the book"] = search?.ToUpper(); // Store the uppercased DepartementName in a local variable
            ViewData["checking"] = false;
            if (!string.IsNullOrEmpty(search))
            {
                Borrowlist = borrowRepo.GetUsingSearchWordforUserName(search);
                if (Borrowlist == null)
                {
                    ViewData["checking"] = true;
                    Borrowlist = borrowRepo.GetAll();
                }
            }
            else
            {
                Borrowlist = borrowRepo.GetAll();
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = Borrowlist.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(Borrowlist.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BorrowViewModel borrowViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    for (int i = 0; i < borrowViewModel.BookIdForBorrow.Count(); i++)
                    {
                        BorrowBook borrow = new BorrowBook();
                        borrow.BorrowedDate = borrowViewModel.BorrowedDate;
                        borrow.ReturnDate = borrowViewModel.ReturnDate;
                        borrow.ApplicationUserId = borrowViewModel.ApplicationUserId;
                        ApplicationUser user = await userManeger.FindByIdAsync(borrow.ApplicationUserId);
                        if (user != null)
                        {
                            foreach (var book in borrowViewModel.books)
                            {
                                user.BorrowedBooks.Add(book);
                            }
                        }
                        borrow.BookId = borrowViewModel.BookIdForBorrow[i];
                        borrowRepo.Insert(borrow);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ApplicationUserId", "Select the User for borrowing process");
                    ModelState.AddModelError("BookId", "Select the Book for borrowing process");
                }
                // Redirect to the index action for further modifications
                return RedirectToAction("index");
            }
            //ModelState.AddModelError("", "Please select an image.");
                return View(borrowViewModel);
        }
        [HttpGet]
       public async Task<IActionResult> ShowUsers(string select)
        {
            List<ApplicationUser> list = null;
            if(select== "Student")
            {
                var Studentlist =await userManeger.GetUsersInRoleAsync("Student");
                list = Studentlist.ToList();

            }
            else if (select == "Teacher")
            {
                var Teacherlist = await userManeger.GetUsersInRoleAsync("Teacher");
                list = Teacherlist.ToList();
            }
            return Json(list);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Details(int id)
        {
            // Retrieve the BorrowBook entity from your data store based on the provided ID
            BorrowBook book = borrowRepo.GetById(id);

            if (book == null)
            {
                return NotFound(); // Or handle the case when the BorrowBook is not found
            }
            BorrowViewModel borrrowVM = new BorrowViewModel();
            borrrowVM.BorrowBookID = book.BorrowBookID;
            borrrowVM.BorrowedDate = book.BorrowedDate;
            borrrowVM.ReturnDate = book.ReturnDate;
            borrrowVM.ApplicationUserId = book.ApplicationUserId;
            borrrowVM.BookId = book.BookId;
            return View(borrrowVM);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Delete(int id)
        {
            // Retrieve the BorrowBook entity from your data store based on the provided ID
            BorrowBook book = borrowRepo.GetById(id);

            if (book == null)
            {
                return NotFound(); // Or handle the case when the BorrowBook is not found
            }
            BorrowViewModel borrrowVM = new BorrowViewModel();
            borrrowVM.BorrowBookID = book.BorrowBookID;
            borrrowVM.BorrowedDate = book.BorrowedDate;
            borrrowVM.ReturnDate = book.ReturnDate;
            borrrowVM.ApplicationUserId = book.ApplicationUserId;
            borrrowVM.BookId = book.BookId;
            return View(borrrowVM);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Mvc.ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Retrieve the BorrowBook entity from your data store based on the provided ID
            BorrowBook book = borrowRepo.GetById(id);

            if (book == null)
            {
                return NotFound(); // Or handle the case when the BorrowBook is not found
            }
            BorrowViewModel borrrowVM = new BorrowViewModel();
            borrrowVM.BorrowBookID = book.BorrowBookID;
            borrrowVM.BorrowedDate = book.BorrowedDate;
            borrrowVM.ReturnDate = book.ReturnDate;
            borrrowVM.ApplicationUserId = book.ApplicationUserId;
            borrrowVM.BookId = book.BookId;

            // Remove the BorrowBook entity from the data store
            borrowRepo.Release(book);
            return RedirectToAction("Index");
        }
    }
}
