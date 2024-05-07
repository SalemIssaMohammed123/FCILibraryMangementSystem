using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Test.Models;
using Test.ViewModels;
using Microsoft.AspNetCore.Authorization;
namespace Test.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager) {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public IActionResult index()
        {

            List<IdentityRole> roles = roleManager.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RoleViewModel roleVM)
        {
            if(ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();
                role.Name = roleVM.RoleName;
                IdentityResult result= await roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                      return RedirectToAction("Index"); 
                }
                else
                {
                    foreach (var Erroritem in result.Errors)
                    {
                        ModelState.AddModelError("", Erroritem.Description);
                    }
                }
            }
            return View(roleVM);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string name)
        {
            var role = await roleManager.FindByNameAsync(name);
            if (role == null)
            {
                // Handle the case when the role does not exist
                return NotFound();
            }

            var usersInRole = await userManager.GetUsersInRoleAsync(role.Name);

            // Perform a join to get the user names associated with the role
            var usersWithRole = from user in usersInRole
                                join userRole in userManager.Users
                                    on user.Id equals userRole.Id
                                select userRole.UserName;
            RoleDeleteViewModel roleDeleteViewModel = new RoleDeleteViewModel();
            roleDeleteViewModel.RoleName = role.Name;
            roleDeleteViewModel.UsersWithRole= usersWithRole.ToList();
            roleDeleteViewModel.RoleId = role.Id;


            return View(roleDeleteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDeleted(string name) 
        {
            var role = await roleManager.FindByNameAsync(name);
            if (role == null)
            {
                // Handle the case when the role does not exist
                return NotFound();
            }

            IdentityResult result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach(var errorItem in  result.Errors)
                {
                    ModelState.AddModelError("", errorItem.Description);
                }
            }
            return View(result);
           

        }
    }
}
