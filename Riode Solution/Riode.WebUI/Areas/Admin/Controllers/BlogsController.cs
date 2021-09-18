using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly RiodeDBContext db;

        public BlogsController(RiodeDBContext context)
        {
            db = context;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index()
        {
            var riodeDBContext = db.Blogs.Include(b => b.Category)
                .Include(i=>i.Images)
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
                .Include(c=>c.Images)
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
        public async Task<IActionResult> Create([Bind("Name,Description,CategoryId,PublishedDate,Id,CreatedByUserId,CreatedDate,DeleteByUserId,DeleteDate")] Blog blog)
        {
            if (ModelState.IsValid)
            {
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
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,CategoryId,PublishedDate,Id,CreatedByUserId,CreatedDate,DeleteByUserId,DeleteDate")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(blog);
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
                .Include(b=>b.Images)
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
