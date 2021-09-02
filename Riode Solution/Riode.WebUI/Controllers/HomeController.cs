using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Models.Entities;

namespace Riode.WebUI.Controllers
{
    public class HomeController : Controller
    {
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

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult BlogSingle()
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
