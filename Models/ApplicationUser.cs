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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BorrowBook> BorrowedBooks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportForBook> ReportedBook { get; set; }

    }
}
