using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Riode.WebUI.Models.Entities
{
    public class Products :BaseEntity
    {
        [Required]
        public String Name { get; set; }
        public String SKUCode { get; set; }
        public int BrandId { get; set; }
        public virtual Brands Brand { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Images> Images { get; set; }
        public String ShortDescription { get; set; }
        public String Description { get; set; }
    }
}
