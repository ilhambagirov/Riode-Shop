﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Riode.WebUI.Models.Entities
{
    public class Colors:BaseEntity
    { 
        [Required]
        public String Name { get; set; }
        public String Description { get; set; }
    }
}