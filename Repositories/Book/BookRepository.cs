using Microsoft.EntityFrameworkCore;
using Test.Models;
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
        public ICollection<Test.Models.Book> GetAllAsListForBorrow()
        {
            return Db.Books.Where(b=>b.Count > 0).ToList();
        }

        public IQueryable<Test.Models.Book> GetAllUsingSearchWord(string search)
        {
            return Db.Books.Include(b => b.Author).Include(b => b.Publisher).Include(b => b.Departement).Where(b => b.BookTitle.ToUpper().StartsWith(search) || b.Author.AuthorName.ToUpper().StartsWith(search) || b.Author.AuthorName.ToUpper().StartsWith(search));
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
            var existingBook = GetById(BookID);
            // Check if the Book ISBN is being changed
            if (existingBook.ISBN == ISBN)
                return true;
            var matchISBN = Db.Books.Where(b => b.BookID != BookID).FirstOrDefault(b => b.ISBN == ISBN);
            if (matchISBN == null)
                return true;
            else
                return false;
        }
        public bool Check_BookISBN_UniqueForCreate(string ISBN)
        {
            var book = Db.Books.FirstOrDefault(b => b.ISBN == ISBN);
            if (book != null)
                return false;
            else
                return true;
        }
        public Test.Models.Book GetById(int id)
        {
            return Db.Books.Include(b => b.Departement).Include(b => b.Author).Include(b => b.Author).FirstOrDefault(b => b.BookID == id);
        }
        public void Insert(Test.Models.Book book)
        {
            book.Author = Db.Authors.FirstOrDefault(b => b.AuthorID == book.AuthorID);
            book.Publisher = Db.Publishers.FirstOrDefault(b => b.PublisherID == book.PublisherID);
            book.Departement = Db.Departements.FirstOrDefault(b => b.DepartementID == book.DepartmentID);
            Db.Books.Add(book);
            Db.SaveChanges();
        }
        public void Update(Test.Models.Book existingBook)
        {
            Db.Books.Update(existingBook);
            Db.SaveChanges();
        }
        public void Delete(Test.Models.Book book)
        {
            Db.Books.Remove(book);
            Db.SaveChanges();
        }
        public string GetDepartementNameByID(int DepartementID)
        {
            return Db.Departements.FirstOrDefault(d => d.DepartementID == DepartementID).DepartementName;
        }
        public string GetPublisherNameByID(int PublisherID)
        {
            return Db.Publishers.FirstOrDefault(d => d.PublisherID == PublisherID).PublisherName;
        }
        public string GetAuthorNameByID(int AuthorID)
        {
            return Db.Authors.FirstOrDefault(d => d.AuthorID == AuthorID).AuthorName;
        }
    }
}
