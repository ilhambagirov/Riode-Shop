using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Models.Entities
{
    public class SpesificationCategoryItem : BaseEntity
    {

        public int SpesificationId { get; set; }
        public virtual Spesification Spesification { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
