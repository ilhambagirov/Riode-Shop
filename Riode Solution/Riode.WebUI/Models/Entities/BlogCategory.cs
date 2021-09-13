﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Models.Entities
{
    public class BlogCategory : BaseEntity
    {
        public int? ParentId { get; set; }

        public virtual BlogCategory Parent { get; set; }

        public virtual ICollection<BlogCategory> Children { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}