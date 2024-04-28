using System.ComponentModel.DataAnnotations;

namespace Test.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
