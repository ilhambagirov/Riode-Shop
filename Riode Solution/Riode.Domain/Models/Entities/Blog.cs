using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.Domain.Models.Entities
{
    public class Blog : BaseEntity
    {

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string ImagePath { get; set; }

        public DateTime? PublishedDate { get; set; }
    }
}
