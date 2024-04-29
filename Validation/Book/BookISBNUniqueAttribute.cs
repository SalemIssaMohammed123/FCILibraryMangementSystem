using System.ComponentModel.DataAnnotations;
using Test.Models;
using Test.Repositories.Book;
using Test.ViewModels;
namespace Test.Validation
{
    public class BookISBNNameUniqueAttribute : ValidationAttribute
    {
        Context Db = new Context();
        public string ErrorMessage { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var book = (BookViewModelForEdit)validationContext.ObjectInstance;

            var existingBook = Db.Books.FirstOrDefault(b => b.BookID == book.BookID);
            // Check if the Departement name is being changed
            if (existingBook.ISBN == (string)value)
                return ValidationResult.Success;
            var matchName = Db.Books.Where(b => b.BookID != book.BookID).FirstOrDefault(b => b.ISBN == (string)value);
            if (matchName == null)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);
        }
    }
}
