using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.Domain.Models.Entities
{
    public class Spesification : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<SpesificationCategoryItem> SpesificationCategoryItem { get; set; }
    }
}
