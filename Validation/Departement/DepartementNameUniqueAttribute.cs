using System.ComponentModel.DataAnnotations;
using Test.Models;
using Test.Repositories.Departement;
using Test.ViewModels;
namespace Test.Validation
{
    public class DepartementNameUniqueAttribute : ValidationAttribute
    {
        Context Db = new Context();
        public string ErrorMessage { get; set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var departement = (DepartementViewModelForEdit)validationContext.ObjectInstance;

            var existingDepartement = Db.Departements.FirstOrDefault(d => d.DepartementID == departement.DepartementID);
            // Check if the Departement name is being changed
            if (existingDepartement.DepartementName == (string)value)
                return ValidationResult.Success;
            var matchName = Db.Departements.Where(d => d.DepartementID != departement.DepartementID).FirstOrDefault(d => d.DepartementName == (string)value);
            if (matchName == null)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);
        }
    }
}
