using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using System.Linq;

namespace Riode.WebUI.Controllers
{
    public class HomeController : Controller
    {
        readonly RiodeDBContext db;
        public HomeController(RiodeDBContext db)
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
            var faqs = db.FAQs
                .Where(f => f.DeleteByUserId == null).ToList();
            return View(faqs);
        }

        public IActionResult notFoundPage()
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
            if (ModelState.IsValid)
            {
                db.ContactPosts.Add(contact);
                db.SaveChanges();

                ViewBag.Message = "Successfully";
                ModelState.Clear();
                return Json(new
                {
                    error = false,
                    message= "Successfully"
                });
            }
            return Json(new
            {
                error = true,
                message = "Try Again"
            });
        }
    }
}
