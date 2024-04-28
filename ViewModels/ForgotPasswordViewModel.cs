using System.ComponentModel.DataAnnotations;

namespace Test.ViewModels
{
	public class ForgotPasswordViewModel
	{
		[Required]
		[Display(Name = "UserName")]
		public string UserName { get; set; }

		[Required]
        [Display(Name = "Email Address")]
		[EmailAddress]
		public string Email { get; set; }

    }
}
