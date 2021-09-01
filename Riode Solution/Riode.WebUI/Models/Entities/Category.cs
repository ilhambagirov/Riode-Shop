﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Riode.WebUI.Models.Entities
{
    public class Category :BaseEntity
    {
        public int? ParentId { get; set; }

        public virtual Category Parent { get; set; }

        public virtual ICollection<Category> Children { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
