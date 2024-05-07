using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
namespace Test.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }
        [Required(ErrorMessage = "Please,enter the bookTitle!!!.")]
        [MaxLength(30, ErrorMessage = "Please,the bookTitle must not contain more than or equal 30 characters!!!.")]
        [MinLength(10, ErrorMessage = "Please,the bookTitle must not contain less than 10 characters!!!.")]
        public string BookTitle { get; set; }
        [Required(ErrorMessage = "Please,enter the ISBN for this book")]
        //custom validation
        [Remote("Check_BookISBN_Unique", "Book", AdditionalFields = "BookID", ErrorMessage = "It is a unique identifier for books,Write another ISBN!!!.")]
        [RegularExpression("^[A-Z]{2,3}_[0-9]{4}$", ErrorMessage = "The format should be two or three uppercase letters followed by an underscore and four digits.")]
        public string ISBN { get; set; }
        [Display(Name = "The Number of Available Amountof books in the Library of this book")]
        [Required(ErrorMessage = "The Number of Available Amountof books in the Library is required")]
        [Range(0, 4, ErrorMessage = "The Number of Available Amountof books in the Library must be a positive number and the target amount is between one book to four books!!!.")]
        public int Count { get; set; }
        [ForeignKey("Publisher")]
        [Display(Name = "Publisher")]
        public int PublisherID { get; set; }
        [ForeignKey("Department")]
        [Display(Name = "Departement")]
        public int DepartmentID { get; set; }
        [Required(ErrorMessage = "Number of pages is required")]
        [Range(250, 1500, ErrorMessage = "Number of pages must be a positive number")]
        public int NoOfPage { get; set; }
        [ForeignKey("Author")]
        [Display(Name = "Author")]
        public int AuthorID { get; set; }
        [Required(ErrorMessage = "Please,upload the BookImage")]
        [Display(Name = "BookImage")]
        [RegularExpression("^[a-zA-Z]+\\.[a-zA-Z]+.(jpg|png)$")]
        public string ImageUrl { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual Departement Departement { get; set; }
        public virtual Author Author { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BorrowBook> BorrowBooks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportForBook> ReportedBooks { get; set; }
    }
}
