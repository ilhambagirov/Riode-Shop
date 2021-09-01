using System;
using System.ComponentModel.DataAnnotations;

namespace Riode.WebUI.Models.Entities
{
    public class Images : BaseEntity
    {
        [Required]
        public String FileName { get; set; }
        public bool IsMain { get; set; }
        public int ProductId { get; set; }
        public Products Product { get; set; }
    }
}
