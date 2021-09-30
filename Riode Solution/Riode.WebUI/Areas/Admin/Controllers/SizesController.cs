using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Riode.WebUI.AppCode.Application.ProductSizeModule;
using Riode.WebUI.Models.DataContext;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizesController : Controller
    {
        private readonly RiodeDBContext _context;
        readonly IMediator mediator;

        public SizesController(RiodeDBContext context, IMediator mediator)
        {
            _context = context;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.sizes.index")]
        public async Task<IActionResult> Index(ProductSizePagedQuery query)
        {
            var sizes = await mediator.Send(query);
            return View(sizes);
        }

        [Authorize(Policy = "admin.sizes.details")]
        public async Task<IActionResult> Details(ProductSizeSingleQuery query)
        {
            var size = await mediator.Send(query);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        [Authorize(Policy = "admin.sizes.create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sizes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductSizeCreateCommand command)
        {
            var response = await mediator.Send(command);
            if (response > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }

        [Authorize(Policy = "admin.sizes.edit")]
        public async Task<IActionResult> Edit(ProductSizeSingleQuery query)
        {
            var size = await mediator.Send(query);
            if (size == null)
            {
                return NotFound();
            }
            ProductSizeViewModel vm = new();
            vm.Id = size.Id;
            vm.Name = size.Name;
            vm.Description = size.Description;
            vm.Abbr = size.Abbr;
            return View(vm);
        }

        // POST: Admin/Sizes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductSizeEditCommand command)
        {
            var response = await mediator.Send(command);
            if (response > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }

        [HttpPost]
        [Authorize(Policy = "admin.sizes.delete")]
        public async Task<IActionResult> Delete(ProductSizeDeleteCommand command)
        {
            var response = await mediator.Send(command);

            return Json(response);
        }
    }
}
