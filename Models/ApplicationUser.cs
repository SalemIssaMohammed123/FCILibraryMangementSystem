using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace Test.Models
{
    public class ApplicationUser :IdentityUser
    {
        public string Address { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageUrl { get; set; }


    }
}
