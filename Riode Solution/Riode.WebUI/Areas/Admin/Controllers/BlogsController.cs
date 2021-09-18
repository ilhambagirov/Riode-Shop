using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        readonly IWebHostEnvironment env;
        readonly RiodeDBContext db;

        public BlogsController(RiodeDBContext context, IWebHostEnvironment env)
        {
            db = context;
            this.env = env;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            var riodeDBContext = db.Blogs.Include(b => b.Category)
                .Where(b => b.DeleteByUserId == null);
            return View(await riodeDBContext.ToListAsync());
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await db.Blogs
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Admin/Blogs/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name");
            return View();
        }

        // POST: Admin/Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog, IFormFile file)
        {
            if (file == null)
            {
                ModelState.AddModelError("file", "Not chosen");
            };

            if (ModelState.IsValid)
            {

                var extension = Path.GetExtension(file.FileName);
                blog.ImagePath = $"{Guid.NewGuid()}{extension}";
                var physicalAddress = Path.Combine(env.ContentRootPath,
                    "wwwroot",
                    "uploads",
                    "images",
                    "blog",
                    blog.ImagePath);

                using (var stream = new FileStream(physicalAddress, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(stream);
                }

                db.Add(blog);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name", blog.CategoryId);
            return View(blog);
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await db.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name", blog.CategoryId);
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Blog blog, IFormFile file, string fileTemp)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(fileTemp) && file == null)
            {
                ModelState.AddModelError("file", "Not Chosen");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // db.Update(blog);
                    var entity = await db.Blogs.FirstOrDefaultAsync(b => b.Id == id && b.DeleteByUserId == null);
                    if (entity == null)
                    {
                        return NotFound();
                    }

                    entity.Name = blog.Name;
                    entity.Description = blog.Description;
                    entity.CategoryId = blog.CategoryId;

                    if (file != null)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        blog.ImagePath = $"{Guid.NewGuid()}{extension}";
                        var physicalAddress = Path.Combine(env.ContentRootPath,
                            "wwwroot",
                            "uploads",
                            "images",
                            "blog",
                            blog.ImagePath);

                        using (var stream = new FileStream(physicalAddress, FileMode.Create, FileAccess.Write))
                        {
                            await file.CopyToAsync(stream);
                        }

                        if (!string.IsNullOrEmpty(entity.ImagePath))
                        {
                            System.IO.File.Delete(Path.Combine(env.ContentRootPath,
                                                       "wwwroot",
                                                       "uploads",
                                                       "images",
                                                       "blog",
                                                       entity.ImagePath));
                        }
                        entity.ImagePath = blog.ImagePath;
                    }
                   
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name", blog.CategoryId);
            return View(blog);
        }

        // GET: Admin/Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await db.Blogs
                .Include(b => b.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Admin/Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await db.Blogs.FindAsync(id);
            db.Blogs.Remove(blog);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
            return db.Blogs.Any(e => e.Id == id);
        }
    }
}
