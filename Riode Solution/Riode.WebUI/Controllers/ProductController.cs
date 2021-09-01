using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Details(int id)
        {
            var db = new RiodeDBContext();
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
