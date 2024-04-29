using Test.Models;

namespace Test.Repositories.Departement
{
    public interface IDepartementRepository
    {
        IQueryable<Test.Models.Departement> GetAll();

        List<Test.Models.Departement> GetAllAsList();

        List<Test.Models.Book> GetAllBooksAsList(int DepartementID);

        IQueryable<Test.Models.Departement> GetUsingSearchWord(string search);

        bool CheckDepartementNameUniqueForEdit(string DepartementName, int DepartementID);
        bool CheckDepartementNameUniqueForCreate(string DepartementName);

        Test.Models.Departement GetById(int id);

        void Insert(Test.Models.Departement departement);

        void Update(Test.Models.Departement existingDepartement);

        void Delete(Test.Models.Departement departement);
    }
}
