using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;
namespace Test.Models
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }
        
        public string BookTitle { get; set; }
        
        public string ISBN { get; set; }
        
        public int? Count { get; set; }
        [ForeignKey("Publisher")]
        public int PublisherID { get; set; }
        [ForeignKey("Departement")]
        public int DepartmentID { get; set; }
        
        public int NoOfPage { get; set; }
        [ForeignKey("Author")]
        public int AuthorID { get; set; }
        
        public string ImageUrl { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual Departement Departement { get; set; }
        public virtual Author Author { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BorrowBook> BorrowBooks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReportForBook> ReportedBooks { get; set; }
    }
}
