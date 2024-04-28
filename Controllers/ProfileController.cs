using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Test.Models;
using Test.ViewModels;
namespace Test.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;

        ProfileController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            Claim UserClaim= User.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.NameIdentifier);
            string id= UserClaim.Value.ToString();
            ApplicationUser user= await userManager.FindByIdAsync(id);
            ProfileViewModel myProfile= new ProfileViewModel();
            myProfile.UserName= user.UserName;
            myProfile.ImgUrl = user.ImageUrl;
            return PartialView(myProfile);
        }
    }
}
