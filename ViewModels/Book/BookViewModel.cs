using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Test.Validation;

namespace Test.ViewModels
{
    public class BookViewModel
    {
        public int BookID { get; set; }
        
        [Required(ErrorMessage = "Please,enter the bookTitle!!!.")]
        [MaxLength(30, ErrorMessage = "Please,the bookTitle must not contain more than or equal 30 characters!!!.")]
        [MinLength(10, ErrorMessage = "Please,the bookTitle must not contain less than 10 characters!!!.")]
        public string BookTitle { get; set; }
        [Required(ErrorMessage = "Please,enter the ISBN for this book")]
        //custom validation
        [Microsoft.AspNetCore.Mvc.Remote("Check_BookISBN_Unique", "Book", ErrorMessage = "It is a unique identifier for books,Write another ISBN!!!.")]
        [RegularExpression("^[A-Z]{2,3}_[0-9]{4}$", ErrorMessage = "The format should be two or three uppercase letters followed by an underscore and four digits.")]
        public string ISBN { get; set; }
        [Display(Name = "The Number of Available Amountof books in the Library of this book")]
        [Required(ErrorMessage = "The Number of Available Amountof books in the Library is required")]
        [Range(0, 4, ErrorMessage = "The Number of Available Amountof books in the Library must be a positive number and the target amount is between one book to four books!!!.")]
        public int? Count { get; set; }
        [Display(Name = "Publisher")]
        public int PublisherID { get; set; }
        [Display(Name = "Departement")]
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Number of pages is required")]
        [Range(5, 10, ErrorMessage = "Number of pages must be a positive number between 5 and 10 number")]
        public int NoOfPage { get; set; }
        [Display(Name = "Author")]
        public int AuthorID { get; set; }
        [Required(ErrorMessage = "Please,upload the BookCover")]
        [Display(Name = "BookCover")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new[] { ".png", ".jpg", ".jpeg" })]
        public IFormFile image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
