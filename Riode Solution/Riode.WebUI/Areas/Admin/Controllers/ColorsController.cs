using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.Application.ProductColorModule;
using Riode.Application.ProductSizeModule;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorsController : Controller
    {
        private readonly RiodeDBContext _context;
        readonly IMediator mediator;

        public ColorsController(RiodeDBContext context, IMediator mediator)
        {
            _context = context;
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.colors.index")]
        public async Task<IActionResult> Index(ProductColorPagedQuery query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }

        [Authorize(Policy = "admin.colors.details")]
        public async Task<IActionResult> Details(ProductColorSingleQuery query)
        {
            var color = await mediator.Send(query);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }

        [Authorize(Policy = "admin.colors.create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Colors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.colors.create")]
        public async Task<IActionResult> Create(ProductColorCreateCommand command)
        {
            var response = await mediator.Send(command);
            if (response > 0)
            {
                return RedirectToAction("Index");
            }
            return View(command);
        }

        [Authorize(Policy = "admin.colors.edit")]
        public async Task<IActionResult> Edit(ProductColorSingleQuery query)
        {
            var color = await mediator.Send(query);
            if (color == null)
            {
                return NotFound();
            }
            ProductColorViewModel vm = new();
            vm.Name = color.Name;
            vm.Description = color.Description;
            vm.HexCode = color.HexCode;

            return View(vm);
        }

        // POST: Admin/Colors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.colors.edit")]
        public async Task<IActionResult> Edit([FromRoute] int id ,ProductColorEditCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            if (response > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }

        [HttpPost]
        [Authorize(Policy = "admin.colors.delete")]
        public async Task<IActionResult> Delete(ProductColorDeleteCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }
    }
}
