using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.Application.BrandModule;
using Riode.Domain.Models.DataContext;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly RiodeDBContext _context;
        private readonly IMediator mediatr;

        public BrandsController(RiodeDBContext context, IMediator mediatr)
        {
            _context = context;
            this.mediatr = mediatr;
        }

        [Authorize(Policy="admin.brands.index")]
        public async Task<IActionResult> Index(BrandPagedQuery request)
        {
            var response = await mediatr.Send(request);
            return View(response);
        }

        [Authorize(Policy = "admin.brands.details")]
        public async Task<IActionResult> Details(BrandSingleQuery request)
        {
            var brand = await mediatr.Send(request);

            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        [Authorize(Policy = "admin.brands.create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.brands.create")]
        public async Task<IActionResult> Create(BrandCreateCommand brand)
        {
            var id = await mediatr.Send(brand);
            if (id > 0)
                return RedirectToAction(nameof(Index));

            return View(brand);
        }

        [Authorize(Policy = "admin.brands.edit")]
        public async Task<IActionResult> Edit(BrandSingleQuery request)
        {
            var brand = await mediatr.Send(request);

            if (brand == null)
            {
                return NotFound();
            }
            BrandViewModel vm = new();
            vm.Id = brand.Id;
            vm.Name = brand.Name;
            vm.Description = brand.Description;

            return View(vm);
        }

        // POST: Admin/Brands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.brands.edit")]
        public async Task<IActionResult> Edit([FromRoute] int id,BrandEditCommand request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            int _id = await mediatr.Send(request);

            if (_id > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        [HttpPost]
        [Authorize(Policy = "admin.brands.delete")]
        public async Task<IActionResult> Delete(BrandDeleteCommand request)
        {
            var response = await mediatr.Send(request);

            return Json(response);
        }

     

       
    }
}
