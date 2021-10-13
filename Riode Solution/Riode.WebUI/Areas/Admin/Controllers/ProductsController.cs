using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Riode.Domain.Models.DataContext;
using Riode.Domain.Models.Entities;
using Riode.Domain.Models.FormModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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

        [Authorize(Policy = "admin.product.index")]
        public async Task<IActionResult> Index()
        {
            var riodeDBContext = _context.Products.Include(p => p.Brand).Include(p => p.Category)
                .Include(p => p.Images.Where(i => i.IsMain == true && i.DeleteByUserId == null));
            return View(await riodeDBContext.ToListAsync());
        }

        [Authorize(Policy = "admin.product.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        [Authorize(Policy = "admin.product.create")]
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name");
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            /*  ViewData["SizeId"] = new SelectList(_context.ProductSizeColorCollection.Include(s=>s.Size), "Id", "Name");
              ViewData["CategoryId"] = new SelectList(_context.ProductSizeColorCollection.Include(s => s.Color), "Id", "Name");*/
            return View();
        }

        // POST: Admin/Products/Create
        [Authorize(Policy = "admin.product.create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,SKUCode,BrandId,CategoryId,ShortDescription,Description,Id,CreatedByUserId,CreatedDate,DeleteByUserId,DeleteDate")] Products products, ImageItem[] images)
        {

            if (images == null || !images.Any(i => i.File != null))
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
                        "product",
                        imagePath);

                    using (var stream = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await image.File.CopyToAsync(stream);

                    }
                    products.Images.Add(new Images
                    {
                        IsMain = image.IsMain,
                        FileName = imagePath
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

        [Authorize(Policy = "admin.product.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.Include(p => p.Images)
                .Include(p => p.Images.Where(i => i.DeleteByUserId == null))
                .FirstOrDefaultAsync(p => p.Id == id);
            if (products == null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_context.Brands, "Id", "Name", products.BrandId);
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", products.CategoryId);
            return View(products);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.product.edit")]
        public async Task<IActionResult> Edit(int id, Products products)
        {
            if (id != products.Id)
            {
                return NotFound();
            }




            if (ModelState.IsValid)
            {
                try
                {

                    var entity = await _context.Products
                        .Include(i => i.Images.Where(i => i.DeleteByUserId == null))
                        .FirstOrDefaultAsync(b => b.Id == id && b.DeleteByUserId == null);

                    if (entity == null)
                    {
                        return NotFound();
                    }

                    entity.Name = products.Name;
                    entity.SKUCode = products.SKUCode;
                    entity.ShortDescription = products.ShortDescription;
                    entity.Description = products.Description;
                    entity.CategoryId = products.CategoryId;
                    entity.BrandId = products.BrandId;

                    //deleted

                    foreach (var _id in products.Files.Where(i => i.Id > 0 && string.IsNullOrWhiteSpace(i.TempPath))
                        .Select(f => f.Id)
                        .ToArray())
                    {
                        var oldImage = await _context.Images.FirstOrDefaultAsync(p => p.ProductId == entity.Id && p.Id == _id);
                        if (oldImage == null)
                            continue;

                        oldImage.DeleteDate = DateTime.Now;
                        oldImage.DeleteByUserId = 1;
                    }

                    //notchanged or chnaged condition
                    foreach (var item in products.Files.
                        Where(i => (i.Id > 0 && !string.IsNullOrWhiteSpace(i.TempPath))
                          || i.File != null))
                    {
                        if (item.File == null)
                        {
                            var notCahngedImage = await _context.Images.FirstOrDefaultAsync(p => p.ProductId == entity.Id && p.Id == item.Id);
                            if (notCahngedImage == null)
                                continue;
                            notCahngedImage.IsMain = item.IsMain;
                        }
                        else if (item.File != null)
                        {
                            var extension = Path.GetExtension(item.File.FileName);
                            var imagePath = $"{Guid.NewGuid()}{extension}";
                            var physicalAddress = Path.Combine(env.ContentRootPath,
                                "wwwroot",
                                "uploads",
                                "images",
                                "product",
                               imagePath);

                            using (var stream = new FileStream(physicalAddress, FileMode.Create, FileAccess.Write))
                            {
                                await item.File.CopyToAsync(stream);
                            }

                            entity.Images.Add(new Images
                            {
                                FileName = imagePath,
                                IsMain = item.IsMain
                            });
                        }
                    }

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
                .Include(p => p.Images)
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
