﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.WebUI.Models.Entities
{
    public class Blog : BaseEntity
    {

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<BlogImages> Images { get; set; }
    }
}
