using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Blog(int page = 1)
        {

            int productCount = 3;
            var db = new RiodeDBContext();
            ViewBag.pageCount = Decimal.Ceiling((decimal)db.Blogs.Where(c => c.DeleteByUserId == null).Count() / productCount);
            ViewBag.Page = page;
            var blogs = db.Blogs
             .Include(p => p.Images)
             .Where(c => c.DeleteByUserId == null).Skip((page - 1) * productCount).Take(productCount)
             .ToList();
            return View(blogs);
        }

        public IActionResult BlogSingle(int id)
        {
            var db = new RiodeDBContext();
            var blog = db.Blogs
            .Include(p => p.Images)
            .FirstOrDefault(c => c.DeleteByUserId == null && c.Id == id);
            return View(blog);
        }
    }
}
