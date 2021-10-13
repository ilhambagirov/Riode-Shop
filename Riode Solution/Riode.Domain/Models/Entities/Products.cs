using Riode.Domain.Models.FormModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Riode.Domain.Models.Entities
{
    public class Products : BaseEntity
    {
        [Required]
        public String Name { get; set; }
        public String SKUCode { get; set; }
        public int BrandId { get; set; }
        public virtual Brands Brand { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Images> Images { get; set; }
        public virtual ICollection<ProductSizeColorItem> ProductSizeColorCollection { get; set; }
        public virtual ICollection<SpesificationValues> SpesificationValues { get; set; }
        public String ShortDescription { get; set; }
        public String Description { get; set; }
        [NotMapped]
        public virtual ImageItem[] Files { get; set; }
    }
}
