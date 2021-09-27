using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.AppCode.Application.BlogModule;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        readonly IWebHostEnvironment env;
        readonly RiodeDBContext db;
        readonly IMediator mediator;

        public BlogsController(RiodeDBContext context, IWebHostEnvironment env, IMediator mediator)
        {
            db = context;
            this.env = env;
            this.mediator = mediator;
        }

        // GET: Admin/Blogs
        public async Task<IActionResult> Index(BlogPagedQuery query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }

        // GET: Admin/Blogs/Details/5
        public async Task<IActionResult> Details(BlogSingleQuery query)
        {
            var blog = await mediator.Send(query);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateCommand command)
        {
            var id = await mediator.Send(command);
            if (id > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name", command.CategoryId);
            return View(command);
        }

        // GET: Admin/Blogs/Edit/5
        public async Task<IActionResult> Edit(BlogSingleQuery query)
        {
            var blog = await mediator.Send(query);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name", blog.CategoryId);
            return View(blog);
        }

        // POST: Admin/Blogs/Edit/5
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
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name", blog.CategoryId);
            return View(blog);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BlogDeleteCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }

    }
}
