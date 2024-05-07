namespace Test.Repositories.Book
{
    public interface IBookRepository
    {
        IQueryable<Test.Models.Book> GetAll();

        ICollection<Test.Models.Book> GetAllAsList();

        IQueryable<Test.Models.Book> GetAllUsingSearchWord(string search);

        IQueryable<Test.Models.Book> GetAllUsingSearchWordAndOrderingWithBookDepartementNameWithDescendingOrder(string search);

        IQueryable<Test.Models.Book> GetAllUsingSearchWordAndOrderingWithBookTitle(string search);

        IQueryable<Test.Models.Book> GetAllUsingSearchWordAndOrderingWithNumberOfPagesOfTheBooks(string search);

        bool Check_BookISBN_UniqueForEdit(string ISBN, int BookID);
        bool Check_BookISBN_UniqueForCreate(string ISBN);

        Test.Models.Book GetById(int id);

        void Insert(Test.Models.Book book);

        void Update(Test.Models.Book existingBook);

        void Delete(Test.Models.Book book);
    }
}
