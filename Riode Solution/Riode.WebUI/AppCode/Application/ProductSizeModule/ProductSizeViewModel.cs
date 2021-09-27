using Riode.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.ProductSizeModule
{
    public class ProductSizeViewModel :BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Abbr { get; set; }
    }
}
