using Microsoft.EntityFrameworkCore;
using System.Web.Helpers;
using Test.Models;

namespace Test.Repositories.Author
{
    public class AuthorRepository : IAuthorRepository
    {
        //Dependency Injection for db context
        Test.Models.Context Db;
        public AuthorRepository(Test.Models.Context Db)
        { 
            this.Db = Db;
        }

      public IQueryable<Test.Models.Author> GetAll()
        {
            return from a in Db.Authors.Include(b=>b.Books) select a;
        }
      public List<Test.Models.Author> GetAllAsList()
        {
            return Db.Authors.Include(b => b.Books).ToList();
        }
        public List<Test.Models.Book> GetAllBooksAsList(int AuthorID)
        {
            return Db.Books.Where(b=>b.AuthorID == AuthorID).ToList();
        }
        public IQueryable<Test.Models.Author> GetAllUsingSearchWord(string search)
        {
            return Db.Authors.Include(b => b.Books).Where(a => a.AuthorName.ToUpper().StartsWith(search) || a.DescripTion.ToUpper().StartsWith(search));
        }
      public IQueryable<Test.Models.Author> GetAllUsingSearchWordAndOrderingWithAuthorName(string search)
        {
            return GetAllUsingSearchWord(search).OrderBy(a => a.AuthorName);
        }
      public IQueryable<Test.Models.Author> GetAllUsingSearchWordAndOrderingWithAuthorDescription(string search)
        {
            return GetAllUsingSearchWord(search).OrderByDescending(a => a.DescripTion);
        }
        public IQueryable<Test.Models.Author> GetAllUsingSearchWordAndOrderingWithAuthorImageName(string search)
        {
            return GetAllUsingSearchWord(search).OrderBy(a => a.ImageUrl);
        }
        public bool CheckAuthorNameUniqueForEdit(string AuthorName, int AuthorID)
        {

            var existingAuthor = GetById(AuthorID);
            // Check if the author name is being changed
            if (existingAuthor.AuthorName == AuthorName)
                return true;
            var matchName = Db.Authors.Where(a=>a.AuthorID!=AuthorID).FirstOrDefault(a => a.AuthorName == AuthorName);
                if (matchName == null)
                    return true;
                else
                    return false;
        }
        public bool CheckAuthorNameUniqueForCreate(string AuthorName)
        {

            var author = Db.Authors.FirstOrDefault(a => a.AuthorName == AuthorName);
            if (author != null)
                return false;
            else
                return true;
        }
        public bool CheckAuthorEmailUniqueForEdit(string Email, int AuthorID)
        {
            var existingAuthor = GetById(AuthorID);
            // Check if the author name is being changed
            if (existingAuthor.Email == Email)
                return true;
                var matchName = Db.Authors.Where(a => a.AuthorID != AuthorID).FirstOrDefault(a => a.Email == Email);
                if (matchName == null)
                return true;
            else
                return false;
        }
        public bool CheckAuthorEmailUniqueForCreate(string Email)
        {
            var author = Db.Authors.FirstOrDefault(a => a.Email == Email);
            if (author != null)
                return false;
            else
                return true;
        }
        public Test.Models.Author GetById(int id)
        {
            return Db.Authors.FirstOrDefault(a => a.AuthorID == id);
        }
        public void Insert(Test.Models.Author author)
        {
            Db.Add(author);
            Db.SaveChanges();
        }
        public void Update(Test.Models.Author existingAuthor)
        {
            Db.Update(existingAuthor);
            Db.SaveChanges();
        }
        public void Delete(Test.Models.Author author)
        {
            Db.Authors.Remove(author);
            Db.SaveChanges();
        }

    }
}
