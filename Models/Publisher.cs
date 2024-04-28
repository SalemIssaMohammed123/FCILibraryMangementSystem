using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace Test.Models
{
    public class Publisher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PublisherID { get; set; }
        [Required(ErrorMessage = "Please,enter the publisher name")]
        [MaxLength(30, ErrorMessage = "Please,the publisher name must not contain more than or equal 30 characters")]
        [MinLength(10, ErrorMessage = "Please,the publisher name must not contain less than 10 characters")]
        //custom validation
        [Remote("CheckPublisherNameUnique", "Publisher", AdditionalFields = "PublisherID", ErrorMessage = "PublisherName already exists, please select another name!!.")]
        public string PublisherName { get; set; }
        [Required(ErrorMessage = "Please,enter the Publisher details")]
        [Display(Name = "Description")]
        [MaxLength(250, ErrorMessage = "Please,the publisher details must not contain more than or equal 250 characters")]
        [MinLength(100, ErrorMessage = "Please,the publisher details must not contain less than 100 characters")]
        public string Description { get; set; }
        [Display(Name = "PublisherImage")]
        [RegularExpression("^[a-zA-Z]+\\.[a-zA-Z]+.(jpg|png)$")]
        [Required(ErrorMessage = "Please,upload the PublisherImage")]
        public string ImageUrl { get; set; }
        [DataType(DataType.EmailAddress)]
        [RegularExpression("^\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*$\r\n", ErrorMessage = "Please,ensure that it starts with one or more word characters, followed by an optional sequence of characters including hyphens, plus signs, periods, or single quotes. It then requires an \"@\" symbol, followed by one or more word characters, a period, and one or more word characters for the domain.")]
        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "Please,enter the publisher email for communcation")]
        //custom validation
        [Remote("CheckPublisherEmailUnique", "Publisher", AdditionalFields = "PublisherID", ErrorMessage = "PublisherEmail already exists, please select publisher email!!.")]
        public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Book> Books { get; set; }
    }
}
