using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.ViewModels;
using System;
using System.Linq;

namespace Riode.WebUI.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        readonly RiodeDBContext db;
        public BlogController(RiodeDBContext db)
        {
            this.db = db;
        }
        [Authorize(Policy = "blog.index")]
        public IActionResult Blog(int page = 1)
        {

            int productCount = 3;
            ViewBag.pageCount = Decimal.Ceiling((decimal)db.Blogs.Where(c => c.DeleteByUserId == null && c.PublishedDate != null).Count() / productCount);
            ViewBag.Page = page;

            var blogs = db.Blogs
             .Where(c => c.DeleteDate == null && c.PublishedDate != null).Skip((page - 1) * productCount).Take(productCount)
            .ToList();
             productCount = 3;

            return View(blogs);
        }
        [Authorize(Policy = "blog.details")]
        public IActionResult BlogSingle(int id)
        {

            BlogCat bc = new BlogCat();

            bc.BlogCats = db.Category
               .Include(c => c.Parent)
                .Include(c => c.Children)
                .ThenInclude(c => c.Children)
                .Where(c => c.ParentId == null && c.DeleteByUserId == null).ToList();

            bc.Blogs = db.Blogs
                       .Where(c => c.DeleteByUserId == null && c.PublishedDate != null)
                       .ToList();

            bc.Blog = db.Blogs
            .FirstOrDefault(c => c.DeleteByUserId == null && c.Id == id && c.PublishedDate != null);



            return View(bc);
        }
    }
}
