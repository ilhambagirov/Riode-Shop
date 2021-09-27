using Riode.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.ProductColorModule
{
    public class ProductColorViewModel : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HexCode { get; set; }
    }
}
