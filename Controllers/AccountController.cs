using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net.Mail;
using System.Net;
using Test.Models;
using Test.ViewModels;
using System.Text;

namespace Test.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        //Dependency Injection
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<JsonResult> CheckUserNameUnique(string UserName)
        {
            var isUserNameUnique = await userManager.FindByNameAsync(UserName);
                if (isUserNameUnique == null)
                {
                    return Json(true);
                }
                else
                {
                    return Json(false);
                }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserViewModel UserVM)
        {
            if (ModelState.IsValid)
            {
                //instead check in the database directly
                ApplicationUser userModel =await userManager.FindByNameAsync(UserVM.UserName);
                //Convert the password string to bytes. 
                byte[] passwordBytes = Encoding.UTF8.GetBytes(UserVM.Password);
                //Encode the password bytes using Base64 encoding. 
                string base64Password = Convert.ToBase64String(passwordBytes);
                if (userModel != null)
                {
                    bool result= await userManager.CheckPasswordAsync(userModel, base64Password);
                    if (result)
                    { 
                        //create cookie ID Name with this user and role in the system
                        await signInManager.SignInAsync(userModel, isPersistent: UserVM.RememberMe);
                        return RedirectToAction("index","Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password","UserPassword doesnot match with the existing password with the UserName in the system");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "UserName doesnot exist in the system");
                }
            }
            return View(UserVM);
        }
        [HttpGet]
        public  IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel UserVM)
        {
            if(ModelState.IsValid && UserVM.image != null && UserVM.image.Length > 0)
            {
                // Generate a unique filename based on the person's ID
                string fileName = UserVM.FirstName.ToString() + Path.GetExtension(UserVM.image.FileName);

                // Set the image path as a combination of a directory and the filename
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/EndUser", fileName);

                // Save the image to the specified path
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await UserVM.image.CopyToAsync(stream);
                }

                // Update the user's ImagePath property
                UserVM.ImageUrl = fileName;
                //Mapping from ViewModel to Model
                ApplicationUser user = new ApplicationUser();
                user.UserName = UserVM.UserName;
                user.FirstName = UserVM.FirstName;
                user.LastName = UserVM.LastName;
                user.ImageUrl = UserVM.ImageUrl;
                user.PasswordHash = UserVM.Password;
                user.Address = UserVM.Address;
                IdentityResult result= await userManager.CreateAsync(user,UserVM.Password);
                if (result.Succeeded)
                {
                    IdentityResult identityResult = await userManager.AddToRoleAsync(user, "EndUser");
                    if (identityResult.Succeeded)
                    {
                        //Create Cookie
                        await signInManager.SignInAsync(user, isPersistent: false);  //ID - Name -Role

                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var erroritem in identityResult.Errors)
                        {
                            ModelState.AddModelError("", erroritem.Description);
                        }
                    }
                    
                }
                else
                {
                    foreach (var erroritem in result.Errors)
                    {
                        ModelState.AddModelError("Password", erroritem.Description);
                    }
                }
            }
            return View(UserVM);
        }
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home"); //when create sign in this action updated
        }
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
		[HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
		{
            if (ModelState.IsValid) 
            {
				var user = await userManager.FindByNameAsync(model.UserName);
				if (user != null)
				{
                   
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resetURL= Url.Action("ResetPassword", "Account", new {userId= user.Id,Token=token},Request.Scheme);
					// Sending the resetUrl to the user via email
					var emailBody = $"Click the following link to reset your password: {resetURL}";
					var smtpClient = new SmtpClient
					{
						Host = "smtp.example.com",
						Port = 587,
						EnableSsl = true,
						DeliveryMethod = SmtpDeliveryMethod.Network,
						UseDefaultCredentials = false,
						Credentials = new NetworkCredential("your-email@example.com", "your-password")
					};

					var mailMessage = new MailMessage
					{
						From = new MailAddress("your-email@example.com"),
						To = { model.Email },
						Subject = "Password Reset Request",
						Body = emailBody
					};

                    try
                    {
                        smtpClient.Send(mailMessage);

                        // Show a success message or redirect to a confirmation page
                        return View("ForgotPasswordConfirmation");
                    }
                    catch (SmtpException ex)
                    {
                        // Log or handle the exception accordingly
                        ModelState.AddModelError("", "Failed to send email. Please try again later.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "This user doesnot exist in our system");
                }
			}
            
            return View(model);
		}
        [HttpGet]
        public IActionResult ResetPassword(string userId,string Token)
        {
            ResetPasswordViewModel VM =new ResetPasswordViewModel();
            VM.id = userId;
            VM.Token = Token;
            return View(VM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel VM)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(VM.id);
                if (user != null) 
                {
                    IdentityResult result = await userManager.ResetPasswordAsync(user, VM.Token, VM.Password);
                    if (result.Succeeded)
                    {
                        //Create Cookie
                        await signInManager.SignInAsync(user, false);
                        return RedirectToAction("ResetPasswordConfirmation");
                            
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("",error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "This user doesnot exist in our system");
                }
            }
            return View(VM) ;
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation() 
        {
          return View();
        }
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

    }
}
