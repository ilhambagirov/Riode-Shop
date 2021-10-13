using System.ComponentModel.DataAnnotations;

namespace Riode.Domain.Models.Entities
{
    public class FAQ :BaseEntity
    {
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }
    }
}
