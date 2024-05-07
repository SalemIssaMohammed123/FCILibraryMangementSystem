using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Test.Validation;
using Test.Models;

namespace Test.ViewModels
{
    public class BorrowViewModel
    {
        public int BorrowBookID { get; set; }

        [Display(Name = "BorrowedDate: ")]
        public DateTime BorrowedDate { get; set; }

        [Display(Name = "Return Date: ")]
        public DateTime ReturnDate { get; set; }


        public string ApplicationUserId;
        [Display(Name = "Book")]
        public int BookId { get; set; }
        public List<int> BookIdForBorrow { get; set; }
        
        public List<Book> books { get; set; }
    }
}
