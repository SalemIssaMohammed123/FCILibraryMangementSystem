namespace Test.Repositories.Publisher
{
    public interface IPublisherRepository
    {
        IQueryable<Test.Models.Publisher> GetAll();

        List<Test.Models.Publisher> GetAllAsList();

        List<Test.Models.Book> GetAllBooksAsList(int PublisherID);

        IQueryable<Test.Models.Publisher> GetAllUsingSearchWord(string search);

        IQueryable<Test.Models.Publisher> GetAllUsingSearchWordAndOrderingWithPublisherName(string search);

        IQueryable<Test.Models.Publisher> GetAllUsingSearchWordAndOrderingWithPublisherDescription(string search);

        IQueryable<Test.Models.Publisher> GetAllUsingSearchWordAndOrderingWithPublisherImageName(string search);

        bool CheckPublisherNameUniqueForEdit(string PublisherName, int PublisherID);
        bool CheckPublisherNameUniqueForCreate(string PublisherName);


        bool CheckPublisherEmailUniqueForEdit(string Email, int PublisherID);
        bool CheckPublisherEmailUniqueForCreate(string Email);


        Test.Models.Publisher GetById(int id);

        void Insert(Test.Models.Publisher Publisher);

        void Update(Test.Models.Publisher existingPublisher);

        void Delete(Test.Models.Publisher Publisher);
    }
}
