﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.Entities;
using Riode.WebUI.Models.FormModels;

namespace Riode.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly RiodeDBContext _context;
        private readonly IWebHostEnvironment env;

        public ProductsController(RiodeDBContext context, IWebHostEnvironment env)
        {
            _context = context;
            this.env = env;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var riodeDBContext = _context.Products.Include(p => p.Brand).Include(p => p.Category)
                .Include(p => p.Images.Where(i => i.IsMain == true));
            return View(await riodeDBContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,SKUCode,BrandId,CategoryId,ShortDescription,Description,Id,CreatedByUserId,CreatedDate,DeleteByUserId,DeleteDate")] Products products,ImageItem[] images)
        {

            if (images==null || !images.Any(i=>i.File!=null) )
            {
                ModelState.AddModelError("Images", "Not Chosen");

            }

            if (ModelState.IsValid)
            {

                products.Images = new List<Images>();

                foreach (var image in images.Where(i => i.File != null))
                {
                    string extension = Path.GetExtension(image.File.FileName);//.jpg

                    string imagePath = $"{DateTime.Now:yyyMMMddHHmmss}-{Guid.NewGuid()}{extension}";

                    string physicalPath = Path.Combine(env.ContentRootPath,
                        "wwwroot",
                        "uploads",
                        "images",
                        imagePath);

                    using (var stream = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await image.File.CopyToAsync(stream);

                    }
                    products.Images.Add(new Images
                    {
                        IsMain = image.IsMain,
                        FileName=imagePath
                    });
                }
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", products.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", products.CategoryId);
            return View(products);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", products.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", products.CategoryId);
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,SKUCode,BrandId,CategoryId,ShortDescription,Description,Id,CreatedByUserId,CreatedDate,DeleteByUserId,DeleteDate")] Products products)
        {
            if (id != products.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.Id))
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
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", products.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", products.CategoryId);
            return View(products);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.FindAsync(id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
