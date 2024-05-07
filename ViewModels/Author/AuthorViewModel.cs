using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Test.Validation;

namespace Test.ViewModels
{
    public class AuthorViewModel
    {
        public int AuthorID { get; set; }
        [Required(ErrorMessage = "Please,enter the Author name")]
        [MaxLength(30, ErrorMessage = "Please,the Author name must not contain more than or equal 30 characters")]
        [MinLength(10, ErrorMessage = "Please,the Author name must not contain less than 10 characters")]
        //custom validation
        [Microsoft.AspNetCore.Mvc.Remote("CheckAuthorNameUnique", "Author", ErrorMessage = "AuthorName already exists, please select another name!!.")]
        public string AuthorName { get; set; }
        [Required(ErrorMessage = "Please,enter the Author details")]
        [Display(Name = "Description")]
        [MaxLength(250, ErrorMessage = "Please,the Author details must not contain more than or equal 250 characters")]
        [MinLength(100, ErrorMessage = "Please,the Author details must not contain less than 100 characters")]
        public string DescripTion { get; set; }
        [Required(ErrorMessage = "Please,upload the AuthorImage")]
        [Display(Name = "AuthorImage")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new[] { ".png", ".jpg", ".jpeg" })]
        public IFormFile image { get; set; }
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Please,enter the Author email for communcation")]
        //custom validation
        [Microsoft.AspNetCore.Mvc.Remote("CheckAuthorEmailUnique", "Author", ErrorMessage = "AuthorEmail already exists, please select another email!!.")]
        public string Email { get; set; }
        public string? ImgUrl { get; set; }
    }
}
