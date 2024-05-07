using System.ComponentModel.DataAnnotations;
using Test.Models;
using Test.Repositories.Publisher;
using Test.ViewModels;
namespace Test.Validation
{
    public class PublisherEmailUniqueAttribute : ValidationAttribute
    {
        Context Db = new Context();
        public string ErrorMessage { get; set; }
               protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var publisher = (PublisherViewModelForEdit)validationContext.ObjectInstance;

            var existingPublisher = Db.Publishers.FirstOrDefault(a => a.PublisherID == publisher.PublisherID);
            // Check if the Publisher name is being changed
            if (existingPublisher.Email == (string)value)
                return ValidationResult.Success;
            var matchName = Db.Publishers.Where(p => p.PublisherID != publisher.PublisherID).FirstOrDefault(p => p.Email == (string)value);
            if (matchName == null)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);
        }
    }
}
