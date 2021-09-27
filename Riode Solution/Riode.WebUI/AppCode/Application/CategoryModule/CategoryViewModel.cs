using Riode.WebUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.AppCode.Application.CategoryModule
{
    public class CategoryViewModel : BaseEntity
    {
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
