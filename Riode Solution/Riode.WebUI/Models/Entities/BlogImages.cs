using System.ComponentModel.DataAnnotations;

namespace Riode.WebUI.Models.Entities
{
    public class BlogImages : BaseEntity
    {
        [Required]
        public string Filename { get; set; }
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }
      
    }
}
