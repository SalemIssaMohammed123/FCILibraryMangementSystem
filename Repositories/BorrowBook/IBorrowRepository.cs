namespace Test.Repositories.Book
{
    public interface IBorrowRepository
    {
        IQueryable<Test.Models.BorrowBook> GetAll();

        IQueryable<Test.Models.BorrowBook> GetUsingSearchWordforUserName(string userName);
        Test.Models.BorrowBook GetById(int id);


        void Insert(Test.Models.BorrowBook book);


        void Release(Test.Models.BorrowBook book);

    }
}
