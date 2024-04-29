using Microsoft.EntityFrameworkCore;
using System.Web.Helpers;
using Test.Models;
namespace Test.Repositories.Publisher
{
    public class PublisherRepository : IPublisherRepository
    {

            //Dependency Injection for db context
            Test.Models.Context Db;
            public PublisherRepository(Test.Models.Context Db)
            {
                this.Db = Db;
            }

            public IQueryable<Test.Models.Publisher> GetAll()
            {
                return from p in Db.Publishers.Include(b=>b.Books) select p;
            }
            public List<Test.Models.Publisher> GetAllAsList()
            {
                return Db.Publishers.ToList();
            }
            public List<Test.Models.Book> GetAllBooksAsList(int PublisherID)
            {
                return Db.Books.Where(b => b.PublisherID == PublisherID).ToList();
            }
        public IQueryable<Test.Models.Publisher> GetAllUsingSearchWord(string search)
            {
                return Db.Publishers.Include(b => b.Books).Where(p => p.PublisherName.ToUpper().StartsWith(search) || p.Description.ToUpper().StartsWith(search));
            }
            public IQueryable<Test.Models.Publisher> GetAllUsingSearchWordAndOrderingWithPublisherName(string search)
            {
                return GetAllUsingSearchWord(search).OrderBy(p => p.PublisherName);
            }
            public IQueryable<Test.Models.Publisher> GetAllUsingSearchWordAndOrderingWithPublisherDescription(string search)
            {
                return GetAllUsingSearchWord(search).OrderByDescending(p => p.Description);
            }
            public IQueryable<Test.Models.Publisher> GetAllUsingSearchWordAndOrderingWithPublisherImageName(string search)
            {
                return GetAllUsingSearchWord(search).OrderBy(p => p.ImageUrl);
            }
        public bool CheckPublisherNameUniqueForEdit(string PublisherName, int PublisherID)
        {
            var existingPublisher = GetById(PublisherID);
            // Check if the Publisher name is being changed
            if (existingPublisher.PublisherName == PublisherName)
                return true;
            var matchName = Db.Publishers.Where(p => p.PublisherID != PublisherID).FirstOrDefault(p => p.PublisherName == PublisherName);
            if (matchName == null)
                return true;
            else
                return false;
        }
        public bool CheckPublisherNameUniqueForCreate(string PublisherName)
        {
            var Publisher = Db.Publishers.FirstOrDefault(p => p.PublisherName == PublisherName);
            if (Publisher != null)
                return false;
            else
                return true;
        }
        public bool CheckPublisherEmailUniqueForEdit(string Email, int PublisherID)
        {
            var existingPublisher = GetById(PublisherID);
            // Check if the Publisher name is being changed
            if (existingPublisher.Email == Email)
                return true;
            var matchName = Db.Publishers.Where(p => p.PublisherID != PublisherID).FirstOrDefault(p => p.Email == Email);
            if (matchName == null)
                return true;
            else
                return false;
        }
        public bool CheckPublisherEmailUniqueForCreate(string Email)
        {
            var Publisher = Db.Publishers.FirstOrDefault(p => p.Email == Email);
            if (Publisher != null)
                return false;
            else
                return true;
        }
        public Test.Models.Publisher GetById(int id)
            {
                return Db.Publishers.FirstOrDefault(p => p.PublisherID == id);
            }
            public void Insert(Test.Models.Publisher Publisher)
            {
                Db.Add(Publisher);
                Db.SaveChanges();
            }
            public void Update(Test.Models.Publisher existingPublisher)
            {
                Db.Update(existingPublisher);
                Db.SaveChanges();
            }
            public void Delete(Test.Models.Publisher Publisher)
            {
                Db.Publishers.Remove(Publisher);
                Db.SaveChanges();
            }

        
    }
}