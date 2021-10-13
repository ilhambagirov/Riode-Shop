using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.Domain.Models.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int? CreatedByUserId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? DeleteByUserId { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
