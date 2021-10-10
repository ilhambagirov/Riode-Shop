using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Riode.WebUI.AppCode.Application.BlogModule;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.ViewModels;
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

        [Authorize(Policy = "admin.blogs.index")]
        public async Task<IActionResult> Index(BlogPagedQuery query)
        {

           
            var response = await mediator.Send(query);
            return View(response);
        }

        [Authorize(Policy = "admin.blogs.details")]
        public async Task<IActionResult> Details(BlogSingleQuery query)
        {
            var blog = await mediator.Send(query);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        [Authorize(Policy = "admin.blogs.create")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name");
            return View();
        }

        // POST: Admin/Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.blogs.create")]
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

        [Authorize(Policy = "admin.blogs.edit")]
        public async Task<IActionResult> Edit(BlogSingleQuery query)
        {
            var blog = await mediator.Send(query);
            if (blog == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name", blog.CategoryId);
            BlogViewModel vm = new();
            vm.Id = blog.Id;
            vm.Name = blog.Name;
            vm.Description = blog.Description;
            vm.CategoryId = blog.CategoryId;
            vm.fileTemp = blog.ImagePath;
            return View(vm);
        }

        // POST: Admin/Blogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.blogs.edit")]
        public async Task<IActionResult> Edit([FromRoute]int id ,BlogEditCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var _id = await mediator.Send(command);
           
            if (id > 0)
            {
                return RedirectToAction(nameof(Index));
            }


            ViewData["CategoryId"] = new SelectList(db.Category, "Id", "Name", command.CategoryId);
            return View(command);
        }

        [HttpPost]
        [Authorize(Policy = "admin.blogs.delete")]
        public async Task<IActionResult> Delete(BlogDeleteCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }

    }
}
