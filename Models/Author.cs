using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models
{
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorID { get; set; }
        [Required(ErrorMessage ="Please,enter the Author name")]
        [MaxLength(30, ErrorMessage = "Please,the Author name must not contain more than or equal 30 characters")]
        [MinLength(10, ErrorMessage = "Please,the Author name must not contain less than 10 characters")]
        //custom validation
        [Remote("CheckAuthorNameUnique","Author", AdditionalFields = "AuthorID",ErrorMessage = "AuthorName already exists, please select another name!!.")]
        public string AuthorName { get; set; }
        [Required(ErrorMessage = "Please,enter the Author details")]
        [Display(Name = "Description")]
        [MaxLength(250, ErrorMessage = "Please,the Author details must not contain more than or equal 250 characters")]
        [MinLength(100, ErrorMessage = "Please,the Author details must not contain less than 100 characters")]
        public string DescripTion { get; set; }
        [Required(ErrorMessage = "Please,upload the AuthorImage")]
        [Display(Name = "AuthorImage")]
        [RegularExpression("^[a-zA-Z]+\\.[a-zA-Z]+.(jpg|png)$")]
        public string ImageUrl { get; set; }
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$\r\n", ErrorMessage = "Please,ensure that it starts with one or more word characters, followed by an optional sequence of characters including hyphens, plus signs, periods, or single quotes. It then requires an \"@\" symbol, followed by one or more word characters, a period, and one or more word characters for the domain.")]
        [Display(Name ="Email Address")]
        [Required(ErrorMessage = "Please,enter the Author email for communcation")]
        //custom validation
        [Remote("CheckAuthorEmailUnique", "Author", AdditionalFields = "AuthorID", ErrorMessage = "AuthorEmail already exists, please select another email!!.")]
        public string Email { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Book> Books { get; set; }
    }
}
