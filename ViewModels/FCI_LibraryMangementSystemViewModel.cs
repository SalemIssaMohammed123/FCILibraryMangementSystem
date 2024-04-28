using Test.Models;

namespace Test.ViewModels
{
    public class FCI_LibraryMangementSystemViewModel
    {
        public ICollection<Author> Authors { get; set; }

        public ICollection<Publisher> Publishers { get; set; }

        public ICollection<Departement> Departements { get; set;}
    }
}
