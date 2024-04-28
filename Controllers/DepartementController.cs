using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;
using Test.Repositories.Departement;
using static System.Reflection.Metadata.BlobBuilder;

namespace Test.Controllers
{
    public class DepartementController : Controller
    {
        IDepartementRepository DeptRepo;
        public DepartementController(IDepartementRepository DeptRepo)
        {
            this.DeptRepo = DeptRepo;
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Index(string? search)
        {
            var Departementlist = DeptRepo.GetAll();
            TempData["search for specific departement"] = search?.ToUpper(); // Store the uppercased DepartementName in a local variable
            ViewData["checking"] = false;
            if (!string.IsNullOrEmpty(search))
            {
                Departementlist = DeptRepo.GetUsingSearchWord(search);
                if (Departementlist == null)
                {
                    ViewData["checking"] = true;
                    Departementlist = DeptRepo.GetAll();
                }
            }
            else
            {
                Departementlist = DeptRepo.GetAll();
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = Departementlist.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(Departementlist.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;

            return View();
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public Microsoft.AspNetCore.Mvc.IActionResult Search(string search)
        {
            var Departementlist = DeptRepo.GetAll();
            TempData["search for specific departement"] = search?.ToUpper(); // Store the uppercased DepartementName in a local variable
            ViewData["checking"] = false;
            if (!string.IsNullOrEmpty(search))
            {
                Departementlist = DeptRepo.GetUsingSearchWord(search);
                if (Departementlist == null)
                {
                    ViewData["checking"] = true;
                    Departementlist = DeptRepo.GetAll();
                }
            }
            else
            {
                Departementlist = DeptRepo.GetAll();
            }
            int pageSize = 10; // Number of items per page
            int pageNumber = TempData.ContainsKey("PageNumber") ? (int)TempData["PageNumber"] : 1;
            var paginatedList = Departementlist.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewData["PageNumber"] = pageNumber;
            ViewData["TotalPages"] = (int)Math.Ceiling(Departementlist.ToList().Count() / (double)pageSize);
            ViewData["paginatedList"] = paginatedList;
            return PartialView("_SearchPartial");
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public Microsoft.AspNetCore.Mvc.JsonResult CheckDepartementNameUnique(string DepartementName, int DepartementID)
        {
            //for editting 
            bool isDepartementNameUnique = DeptRepo.CheckDepartementNameUniqueForEdit(DepartementName, DepartementID);// Logic to check if the DepartementName is unique in the database
            //for new
            if (DepartementID == 0)
                isDepartementNameUnique = DeptRepo.CheckDepartementNameUniqueForCreate(DepartementName);
            if (isDepartementNameUnique == true)
                return Json(true);
            return Json(false);
        }
        //add Publisher to system
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Create()
        {
            return View(new Departement());
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Departement departement)
        {
            if (ModelState.IsValid)
            {
                // Save the departement entity to your data store
               DeptRepo.Insert(departement);

                // Redirect to the index action for further modifications
                return RedirectToAction("index");
            }
            else
            {
                return View(departement);
            }
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Edit(int id)
        {
            // Retrieve the Publisher entity from your data store based on the provided ID
            Departement departement = DeptRepo.GetById(id);

            if (departement == null)
            {
                return NotFound(); // Or handle the case when the departement is not found
            }

            return View(departement);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Departement departement, int id)
        {
            if (ModelState.IsValid && departement != null)
            {
                // Retrieve the existing departement entity from your data store based on the Publisher's ID
                Departement existingDepartement = DeptRepo.GetById(id);

                if (existingDepartement == null)
                {
                    return NotFound(); // Or handle the case when the Publisher is not found
                }
                // Update other properties of the existing Publisher with the new values
                existingDepartement.DepartementName = departement.DepartementName;
                // Update other properties as needed

                // Update the existing departement entity in your data store
                DeptRepo.Update(existingDepartement);

                // Redirect to the index action for further modifications
                return RedirectToAction("Index");
            }

            return View(departement);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Details(int id)
        {
            // Retrieve the Departement entity from your data store based on the provided ID
            Departement departement = DeptRepo.GetById(id);

            if (departement == null)
            {
                return NotFound(); // Or handle the case when the departement is not found
            }

            return View(departement);
        }
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public IActionResult Delete(int id)
        {
            // Retrieve the Departement entity from your data store based on the provided ID
            Departement departement = DeptRepo.GetById(id);

            if (departement == null)
            {
                return NotFound(); // Or handle the case when the departement is not found
            }

            return View(departement);
        }
        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Mvc.ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Retrieve the Departement entity from your data store based on the provided ID
            Departement departement = DeptRepo.GetById(id);

            if (departement == null)
            {
                return NotFound(); // Or handle the case when the departement is not found
            }

            // Remove the departement entity from the data store
            DeptRepo.Delete(departement);
            return RedirectToAction("Index");
        }
    }
}
