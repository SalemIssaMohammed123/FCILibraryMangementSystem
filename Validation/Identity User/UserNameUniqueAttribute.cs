using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Test.Models;
using Test.Repositories.Author;
using Test.ViewModels;
namespace Test.Validation
{
    public class UserNameUniqueAttribute : ValidationAttribute
    {
        Context Db = new Context();
        UserManager<ApplicationUser> userManager;
        public string ErrorMessage { get; set; }
        public UserNameUniqueAttribute()
        {
        }
        public UserNameUniqueAttribute(UserManager<ApplicationUser> userManager)
        {
        this.userManager = userManager;
        }

        protected  override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var user = (RegisterUserViewModelForEdit)validationContext.ObjectInstance;

            var userWithId = Db.Users.FirstOrDefault(u => u.Id == user.UserId);
            if (userWithId != null && userWithId.UserName == (string)value)
            {
                // The user is editing their profile and the username remains unchanged,
                // so it is still considered unique.
                return ValidationResult.Success;
            }

            var userWithSameName = Db.Users.FirstOrDefault(u => u.UserName == (string)value);
            if (userWithSameName != null && userWithSameName.Id != user.UserId)
            {
                // The username belongs to another user, so it is not unique.
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
