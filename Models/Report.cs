using System.ComponentModel.DataAnnotations.Schema;

namespace Test.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        [ForeignKey("book")]
        public int book_id { get; set; }
        public string report { get; set; }
        public virtual Book book { get; set; }
    }
}
