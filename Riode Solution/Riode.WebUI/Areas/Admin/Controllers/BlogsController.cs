using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Riode.WebUI.AppCode.Application.BlogModule;
using Riode.WebUI.Models.DataContext;
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
        public async Task<IActionResult> Delete(BlogDeleteCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }

    }
}
