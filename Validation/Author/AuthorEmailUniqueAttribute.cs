using System.ComponentModel.DataAnnotations;
using Test.Models;
using Test.Repositories.Author;
using Test.ViewModels;
namespace Test.Validation
{
    public class AuthorEmailUniqueAttribute : ValidationAttribute
    {
        Context Db = new Context();
        public string ErrorMessage { get; set; }
       
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var author = (AuthorViewModelForEdit)validationContext.ObjectInstance;

            var existingAuthor = Db.Authors.FirstOrDefault(a => a.AuthorID == author.AuthorID);
            // Check if the author name is being changed
            if (existingAuthor.Email == (string)value)
                return ValidationResult.Success;
            var matchName = Db.Authors.Where(a => a.AuthorID != author.AuthorID).FirstOrDefault(a => a.Email == (string)value);
            if (matchName == null)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);
        }
    }
}
