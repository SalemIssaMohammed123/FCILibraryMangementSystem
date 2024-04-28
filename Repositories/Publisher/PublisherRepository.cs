using Microsoft.EntityFrameworkCore;
using System.Web.Helpers;
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
                return !Db.Publishers.Any(p => p.PublisherName == PublisherName && p.PublisherID != PublisherID); ;
            }
            public bool CheckPublisherNameUniqueForCreate(string PublisherName)
            {
                return !Db.Publishers.Any(p => p.PublisherName == PublisherName);
            }
            public bool CheckPublisherEmailUniqueForEdit(string Email, int PublisherID)
            {
                return Db.Publishers.Any(p => p.Email == Email && p.PublisherID != PublisherID);
            }
            public bool CheckPublisherEmailUniqueForCreate(string Email)
            {
                return Db.Publishers.Any(p => p.Email == Email);
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