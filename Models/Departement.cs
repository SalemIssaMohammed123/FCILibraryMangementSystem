using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Test.Models
{
    public class Departement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DepartementID { get; set; }
        [Required(ErrorMessage = "Please,enter the departement name")]
        [MaxLength(20, ErrorMessage = "Please,the departement name must not contain more than or equal 20 characters")]
        [MinLength(3, ErrorMessage = "Please,the departement name must not contain less than 3 characters")]
        //custom validation
        [Remote("CheckDepartementNameUnique", "Departement", AdditionalFields = "DepartementID", ErrorMessage = "DepartementName already exists, please select another name!!.")]
        public string DepartementName { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Book> Books { get; set; }
    }
}
