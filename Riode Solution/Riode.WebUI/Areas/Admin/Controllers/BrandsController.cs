﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.AppCode.Application.BrandModule;
using Riode.WebUI.Models.DataContext;
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

        // GET: Admin/Brands
        public async Task<IActionResult> Index(BrandPagedQuery request)
        {
            var response = await mediatr.Send(request);
            return View(response);
        }

        // GET: Admin/Brands/Details/5
        public async Task<IActionResult> Details(BrandSingleQuery request)
        {
            var brand = await mediatr.Send(request);

            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Admin/Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandCreateCommand brand)
        {
            var id = await mediatr.Send(brand);
            if (id > 0)
                return RedirectToAction(nameof(Index));

            return View(brand);
        }

        // GET: Admin/Brands/Edit/5
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
        public async Task<IActionResult> Edit(BrandEditCommand request)
        {
            int id = await mediatr.Send(request);
            if (id > 0)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(BrandDeleteCommand request)
        {
            var response = await mediatr.Send(request);

            return Json(response);
        }

     

       
    }
}
