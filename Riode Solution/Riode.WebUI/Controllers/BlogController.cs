using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.ViewModels;
using System;
using System.Linq;

namespace Riode.WebUI.Controllers
{
    public class BlogController : Controller
    {
        readonly RiodeDBContext db;
        public BlogController(RiodeDBContext db)
        {
            this.db = db;
        }
        public IActionResult Blog(int page = 1)
        {

            int productCount = 3;
            ViewBag.pageCount = Decimal.Ceiling((decimal)db.Blogs.Where(c => c.DeleteByUserId == null).Count() / productCount);
            ViewBag.Page = page;
            var blogs = db.Blogs
             .Include(p => p.Images)
             .Where(c => c.DeleteByUserId == null && c.PublishedDate != null).Skip((page - 1) * productCount).Take(productCount)
             .ToList();
            return View(blogs);
        }

        public IActionResult BlogSingle(int id)
        {

            BlogCat bc = new BlogCat();

            bc.BlogCats = db.Category
               .Include(c => c.Parent)
                .Include(c => c.Children)
                .ThenInclude(c => c.Children)
                .Where(c => c.ParentId == null && c.DeleteByUserId == null).ToList();

            bc.Blog = db.Blogs
            .Include(p => p.Images)
            .FirstOrDefault(c => c.DeleteByUserId == null && c.Id == id && c.PublishedDate != null);

            bc.Blogs = db.Blogs
           .Include(p => p.Images)
           .Where(c => c.DeleteByUserId == null && c.PublishedDate != null)
           .ToList();

            return View(bc);
        }
    }
}
