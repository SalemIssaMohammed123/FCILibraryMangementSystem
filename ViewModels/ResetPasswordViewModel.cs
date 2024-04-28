using System.ComponentModel.DataAnnotations;

namespace Test.ViewModels
{
    public class ResetPasswordViewModel
    { 
        public string id { get; set; }
        
        public string Token { get; set; }

        [Required]
        [MaxLength(8, ErrorMessage = "Please,the password must not contain more than or equal 8 characters")]
        [MinLength(6, ErrorMessage = "Please,the password must not contain less than 6 characters")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }
}
