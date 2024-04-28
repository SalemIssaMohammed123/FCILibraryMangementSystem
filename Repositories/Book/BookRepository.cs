using Microsoft.EntityFrameworkCore;
namespace Test.Repositories.Book
{
    public class BookRepository : IBookRepository
    {
        Test.Models.Context Db;
        public BookRepository(Test.Models.Context Db) 
        { 
         this.Db = Db;
        }

        public IQueryable<Test.Models.Book> GetAll()
        {
            return from b in Db.Books select b;
        }
        public ICollection<Test.Models.Book> GetAllAsList()
        {
            return Db.Books.ToList();
        }
        public IQueryable<Test.Models.Book> GetAllUsingSearchWord(string search)
        {
            return Db.Books.Include(b => b.Author).Include(b => b.Publisher).Include(b => b.Departement).Where(b => b.BookTitle.ToUpper().StartsWith(search) || b.Author.AuthorName.ToUpper().StartsWith(search) || b.Publisher.PublisherName.ToUpper().StartsWith(search));
        }
        public IQueryable<Test.Models.Book> GetAllUsingSearchWordAndOrderingWithBookDepartementNameWithDescendingOrder(string search)
        {
            return GetAllUsingSearchWord(search).OrderByDescending(b => b.Departement.DepartementName);
        }
        public IQueryable<Test.Models.Book> GetAllUsingSearchWordAndOrderingWithBookTitle(string search)
        {
            return GetAllUsingSearchWord(search).OrderBy(b => b.BookTitle);
        }
        public IQueryable<Test.Models.Book> GetAllUsingSearchWordAndOrderingWithNumberOfPagesOfTheBooks(string search)
        {
            return GetAllUsingSearchWord(search).OrderBy(b => b.NoOfPage);
        }
        public bool Check_BookISBN_UniqueForEdit(string ISBN, int BookID)
        {
            return !Db.Books.Any(b => b.ISBN == ISBN && b.BookID != BookID); ;
        }
        public bool Check_BookISBN_UniqueForCreate(string ISBN)
        {
            return !Db.Books.Any(b => b.ISBN == ISBN);
        }
        public Test.Models.Book GetById(int id)
        {
            return Db.Books.Include(b=>b.Departement).Include(b=>b.Author).Include(b=>b.Publisher).FirstOrDefault(b => b.BookID == id);
        }
        public void Insert(Test.Models.Book book)
        {
            Db.Add(book);
            Db.SaveChanges();
        }
        public void Update(Test.Models.Book existingBook)
        {
            Db.Update(existingBook);
            Db.SaveChanges();
        }
        public void Delete(Test.Models.Book book)
        {
            Db.Books.Remove(book);
            Db.SaveChanges();
        }
    }
}
