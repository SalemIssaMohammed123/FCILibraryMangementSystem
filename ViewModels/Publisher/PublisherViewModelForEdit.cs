using System.ComponentModel.DataAnnotations;
using Test.Validation;

namespace Test.ViewModels
{
    public class PublisherViewModelForEdit
    {
        public int PublisherID { get; set; }
        [Required(ErrorMessage = "Please,enter the Publisher name")]
        [MaxLength(30, ErrorMessage = "Please,the Publisher name must not contain more than or equal 30 characters")]
        [MinLength(10, ErrorMessage = "Please,the Publisher name must not contain less than 10 characters")]
        //custom validation
        [PublisherNameUnique(ErrorMessage = "PublisherName must be unique.")]
        public string PublisherName { get; set; }
        [Required(ErrorMessage = "Please,enter the Publisher details")]
        [Display(Name = "Description")]
        [MaxLength(250, ErrorMessage = "Please,the publisher details must not contain more than or equal 250 characters")]
        [MinLength(100, ErrorMessage = "Please,the publisher details must not contain less than 100 characters")]
        public string Description { get; set; }

        [Display(Name = "PublisherImage")]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new[] { ".png", ".jpg", ".jpeg" })]
        public IFormFile? image { get; set; }

        [DataType(DataType.EmailAddress)]
        [RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$", ErrorMessage = "Please,ensure that it starts with one or more word characters, followed by an optional sequence of characters including hyphens, plus signs, periods, or single quotes. It then requires an \"@\" symbol, followed by one or more word characters, a period, and one or more word characters for the domain.")]
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Please,enter the publisher email for communcation")]
        //custom validation
        [PublisherEmailUnique(ErrorMessage ="PublisherEmail must be unique.")]
        public string Email { get; set; }
        public string? ImgUrl { get; set; }
    }
}
