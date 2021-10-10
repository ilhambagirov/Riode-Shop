using Riode.WebUI.Models.Entities;
using System.Collections.Generic;

namespace Riode.WebUI.Models.ViewModels
{
    public class CategoryViewModels
    {

        public List<Category> Category { get; set; }
        public List<Brands> Brands { get; set; }
        public List<Size> Size { get; set; }
        public List<Colors> Colors { get; set; }
        public List<Products> Products { get; set; }
    }
}
