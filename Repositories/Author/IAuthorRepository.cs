namespace Test.Repositories.Author
{
    public interface IAuthorRepository
    {
        IQueryable<Test.Models.Author> GetAll();

        List<Test.Models.Author> GetAllAsList();

        List<Test.Models.Book> GetAllBooksAsList(int AuthorID);

        IQueryable<Test.Models.Author> GetAllUsingSearchWord(string search);

        IQueryable<Test.Models.Author> GetAllUsingSearchWordAndOrderingWithAuthorName(string search);

        IQueryable<Test.Models.Author> GetAllUsingSearchWordAndOrderingWithAuthorDescription(string search);

        IQueryable<Test.Models.Author> GetAllUsingSearchWordAndOrderingWithAuthorImageName(string search);

        bool CheckAuthorNameUniqueForEdit(string AuthorName, int AuthorID);
        bool CheckAuthorNameUniqueForCreate(string AuthorName);


        bool CheckAuthorEmailUniqueForEdit(string Email, int AuthorID);
        bool CheckAuthorEmailUniqueForCreate(string Email);


        Test.Models.Author GetById(int id);

        void Insert(Test.Models.Author author);

        void Update(Test.Models.Author existingAuthor);

        void Delete(Test.Models.Author author);

    }
}
