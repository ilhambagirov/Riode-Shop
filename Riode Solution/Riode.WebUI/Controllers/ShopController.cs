﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.FormModels;
using Riode.WebUI.Models.ViewModels;
using System;
using System.Linq;

namespace Riode.WebUI.Controllers
{
    public class ShopController : Controller
    {
        readonly RiodeDBContext db;
        public ShopController(RiodeDBContext db)
        {
            this.db = db;
        }
        public IActionResult Index(int page = 1)
        {

            var viewmodel = new CategoryViewModel();
            viewmodel.Category = db.Category
                .Include(c => c.Parent)
                .Include(c => c.Children)
                .ThenInclude(c => c.Children)
                .Where(c => c.ParentId == null && c.DeleteByUserId == null)
                .ToList();

            viewmodel.Brands = db.Brands
               .Where(c => c.DeleteByUserId == null)
               .ToList();

            viewmodel.Colors = db.Colors
              .Where(c => c.DeleteByUserId == null)
              .ToList();

            viewmodel.Size = db.Size
              .Where(c => c.DeleteByUserId == null)
              .ToList();

            /*  int productCount = 2;
              ViewBag.PageCount = Decimal.Ceiling((decimal)db.Products.Where(c => c.DeleteByUserId == null).Count() / productCount);
              ViewBag.Page = page;*/

            viewmodel.Products = db.Products
              .Include(p => p.Images.Where(i => i.IsMain == true))
              .Include(C => C.Brand)
              .Where(c => c.DeleteByUserId == null)/*.Skip((page-1)* productCount).Take(productCount)*///2
              .ToList();


            return View(viewmodel);
        }

        [HttpPost]
        public IActionResult Filter(ShopFilterModel model)
        {

            var query = db.Products
                 .Include(p => p.Images.Where(i => i.IsMain == true))
                 .Include(c => c.Brand)
                 .Include(c => c.Category)
                 .Include(c => c.ProductSizeColorCollection)
                 .Where(c => c.DeleteByUserId == null)
                 .AsQueryable();

            if (model?.Brands?.Count() > 0)
            {
                query = query.Where(p => model.Brands.Contains(p.BrandId));
            }

            if (model?.Sizes?.Count() > 0)
            {
                query = query.Where(p => p.ProductSizeColorCollection.Any(pscc =>model.Sizes.Contains(pscc.SizeId)));
            }

            if (model?.Colors?.Count() > 0)
            {
                query = query.Where(p => p.ProductSizeColorCollection.Any(pscc => model.Colors.Contains(pscc.ColorId)));
            }

            return PartialView("_ProductContainer", query.ToList());

           /* return Json(new
            {
                error = false,
                data = query.ToList()
            });*/
        }

        public IActionResult Details(int id)
        {
            var product = db.Products
             .Include(p => p.Images)
             .FirstOrDefault(c => c.DeleteByUserId == null && c.Id == id);
            if (product == null)
            {
                return NotFound();
            }


            return View(product);
        }

    }
}
