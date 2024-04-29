using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Test.Validation;

namespace Test.ViewModels
{
    public class RegisterUserViewModel
    {
        public string? UserId { get; set; }
        [Required(ErrorMessage = "Please,enter the UserName")]
        [MaxLength(15, ErrorMessage = "Please,the UserName must not contain more than or equal 15 characters")]
        [MinLength(10, ErrorMessage = "Please,the UserName must not contain less than 10 characters")]
        //custom validation
        [Remote("CheckUserNameUnique", "Account", ErrorMessage = "UserName already exists, please select another name!!.")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password",ErrorMessage = "Password confirmation does not match!.")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "The Address field is required.")]
        public string Address { get; set;}

        [Required(ErrorMessage = "The FirstName field is required.")]
        [MaxLength(8, ErrorMessage = "The FirstName field must be a maximum of 8 characters.")]
        [MinLength(3, ErrorMessage = "The FirstName field must be a minimum of 3 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "The LastName field is required.")]
        [MaxLength(8, ErrorMessage = "The LastName field must be a maximum of 8 characters.")]
        [MinLength(3, ErrorMessage = "The LastName field must be a minimum of 3 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please,upload the UserImage")]
        [Display(Name = "UserImage")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new[] { ".png", ".jpg", ".jpeg" })]
        public IFormFile image { get; set; }
        public string? ImageUrl { get; set; }
    }
}
