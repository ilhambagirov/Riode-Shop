using System;
using System.ComponentModel.DataAnnotations;

namespace Riode.Domain.Models.Entities
{
    public class Size :BaseEntity
    {
        [Required]
        public String Name { get; set; }
        public String Abbr { get; set; }
        public String Description { get; set; }
    }
}
