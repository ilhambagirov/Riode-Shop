using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;

namespace Riode.WebUI.Controllers
{
    public class HomeController : Controller
    {
        readonly RiodeDBContext db;
        public HomeController(RiodeDBContext db )
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }


        public IActionResult FAQ()
        {
            return View();
        }

        public IActionResult notFoundPage()
        {
            return View();
        }

        public IActionResult comingsSoon()
        {
            return View();
        }

        public IActionResult Elements()
        {
            return View();
        }


        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(Contact contact)
        {
            return View();
        }
    }
}
