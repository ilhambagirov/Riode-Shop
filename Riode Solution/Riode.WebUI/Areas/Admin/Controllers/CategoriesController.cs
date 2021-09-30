using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Riode.WebUI.AppCode.Application.CategoryModule;
using Riode.WebUI.Models.DataContext;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly RiodeDBContext _context;
        readonly IMediator mediator;

        public CategoriesController(RiodeDBContext context, IMediator mediator)
        {
            _context = context;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.categories.index")]
        public async Task<IActionResult> Index(CategoryPagedQuery query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }

        [Authorize(Policy = "admin.categories.details")]
        public async Task<IActionResult> Details(CategorySingleQuery query)
        {
            var category = await mediator.Send(query);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [Authorize(Policy = "admin.categories.create")]
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Admin/Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateCommand command)
        {
            var response = await mediator.Send(command);
            if (response > 0)
            {
                return RedirectToAction("Index");
            }
            ViewData["ParentId"] = new SelectList(_context.Category, "Id", "Name", command.ParentId);
            return View(response);
        }

        [Authorize(Policy = "admin.categories.edit")]
        public async Task<IActionResult> Edit(CategorySingleQuery query)
        {
            var category = await mediator.Send(query);
            if (category == null)
            {
                return NotFound();
            }
            CategoryViewModel vm = new();
            vm.Name = category.Name;
            vm.ParentId = category.ParentId;
            vm.Description = category.Description;
            ViewData["ParentId"] = new SelectList(_context.Category, "Id", "Name", category.ParentId);
            return View(vm);
        }

        // POST: Admin/Categories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryEditCommand command)
        {
            var response = await mediator.Send(command);

            if (response > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewData["ParentId"] = new SelectList(_context.Category, "Id", "Name", command.ParentId);
            return View(command);
        }

        [HttpPost]
        [Authorize(Policy = "admin.categories.delete")]
        public async Task<IActionResult> Delete(CategoryDeleteCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }


    }
}
