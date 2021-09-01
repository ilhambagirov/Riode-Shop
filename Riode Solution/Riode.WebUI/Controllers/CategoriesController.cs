using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.DataContext;
using Riode.WebUI.Models.ViewModels;
using System.Linq;

namespace Riode.WebUI.Controllers
{
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {

            var viewmodel = new CategoryViewModel();
            var db = new RiodeDBContext();

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

            viewmodel.Products = db.Products
              .Include(p => p.Images.Where(i => i.IsMain == true))
              .Where(c => c.DeleteByUserId == null)
              .ToList();


            return View(viewmodel);
        }

        /*
                public IActionResult ShopListMode()
                {
                    return View();
                }
        */

    }
}
