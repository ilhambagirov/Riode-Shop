using Riode.Domain.Models.Entities;
using System.Collections.Generic;

namespace Riode.Domain.Models.ViewModels
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
