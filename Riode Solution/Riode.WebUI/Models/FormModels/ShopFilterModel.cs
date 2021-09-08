using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Models.FormModels
{
    public class ShopFilterModel
    {
        public List<int> Sizes { get; set; }
        public List<int> Brands { get; set; }
        public List<int> Colors { get; set; }
    }
}
