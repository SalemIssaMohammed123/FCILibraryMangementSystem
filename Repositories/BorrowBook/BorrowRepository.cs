using Microsoft.EntityFrameworkCore;
using Test.Models;
namespace Test.Repositories.Book
{
    public class BorrowRepository : IBorrowRepository
    {
        Test.Models.Context Db;
        public BorrowRepository(Test.Models.Context Db)
        {
            this.Db = Db;
        }
       public IQueryable<Test.Models.BorrowBook> GetAll()
        {
            return from b in Db.BorrowBooks select b;
        }
        public IQueryable<Test.Models.BorrowBook> GetUsingSearchWordforUserName(string userName)
        {
            return Db.BorrowBooks.Include(b => b.ApplicationUser).Include(b => b.Book).Where(b => b.ApplicationUser.UserName.ToUpper().StartsWith(userName));
        }

        public Test.Models.BorrowBook GetById(int id)
        {
            return Db.BorrowBooks.Include(b => b.ApplicationUser).Include(b => b.Book).FirstOrDefault(b => b.BorrowBookID == id);
        }
        public void Insert(Test.Models.BorrowBook book)
        {
            Db.BorrowBooks.Add(book);
            Db.SaveChanges();
        }


       public void Release(Test.Models.BorrowBook book)
        {
            Db.BorrowBooks.Remove(book);
            Db.SaveChanges();
        }

    }
}
