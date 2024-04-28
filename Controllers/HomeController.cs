using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Test.Models;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            TempData["search for specific Author"] = "";
            TempData["sort for search for specific Author"] = "";
            TempData["search for specific publisher"] = "";
            TempData["sort for search for specific publisher"] = "";
            TempData["search for specific departement"] = "";
            TempData["search for specific Book"] = "";
            TempData["sort for search for specific Book"] = "";
            TempData["search for specific Admin"] = "";
            TempData["search for specific Teacher"] = "";
            TempData["search for specific Student"] = "";
            TempData["search for specific EndUser"] = "";
            TempData["search for specific TeacherStudent"] = "";
            TempData["search for specific TeacherAdmin"] = "";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "MVC Project On Relational Database. My Database about FCI Library Management System.";

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

       // [Authorize]
        public ActionResult Vision()
        {
            ViewBag.Message = "FCI Library Vision";

            return View();

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
