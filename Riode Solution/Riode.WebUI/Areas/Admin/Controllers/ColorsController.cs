using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.AppCode.Application.ProductColorModule;
using Riode.WebUI.AppCode.Application.ProductSizeModule;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
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

        // GET: Admin/Colors
        public async Task<IActionResult> Index(ProductColorPagedQuery query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }

        // GET: Admin/Colors/Details/5
        public async Task<IActionResult> Details(ProductColorSingleQuery query)
        {
            var color = await mediator.Send(query);
            if (color == null)
            {
                return NotFound();
            }

            return View(color);
        }

        // GET: Admin/Colors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Colors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductColorCreateCommand command)
        {
            var response = await mediator.Send(command);
            if (response > 0)
            {
                return RedirectToAction("Index");
            }
            return View(command);
        }

        // GET: Admin/Colors/Edit/5
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
        public async Task<IActionResult> Edit(ProductColorEditCommand command)
        {
            var response = await mediator.Send(command);
            if (response > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductColorDeleteCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }
    }
}
