using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Repositories.Departement
{
    public class DepartementRepository : IDepartementRepository
    {
        Context Db;
      public DepartementRepository(Context Db)
        { 
         this.Db = Db;
        }
      public IQueryable<Test.Models.Departement> GetAll()
        {
            return from d in Db.Departements.Include(d => d.Books) select d;
        }
        public List<Test.Models.Departement> GetAllAsList()
        {
            return Db.Departements.ToList();
        }
        public IQueryable<Test.Models.Departement> GetUsingSearchWord(string search)
        {
            return Db.Departements.Where(d => d.DepartementName.ToUpper().StartsWith(search));
        }
      public bool CheckDepartementNameUniqueForEdit(string DepartementName, int DepartementID)
        {
            return !Db.Departements.Any(d => d.DepartementName == DepartementName && d.DepartementID != DepartementID);
        }
      public bool CheckDepartementNameUniqueForCreate(string DepartementName)
        {
            return !Db.Departements.Any(d => d.DepartementName == DepartementName);
        }
      public Test.Models.Departement GetById(int id)
        {
            return Db.Departements.Include(d => d.Books).FirstOrDefault(d => d.DepartementID == id);
        }
      public void Insert(Test.Models.Departement departement)
        {
            Db.Add(departement);
            Db.SaveChanges();
        }
      public void Update(Test.Models.Departement existingDepartement)
        {
            Db.Update(existingDepartement);
            Db.SaveChanges();
        }
      public void Delete(Test.Models.Departement departement)
        {
            Db.Departements.Remove(departement);
            Db.SaveChanges();
        }
    }
}
