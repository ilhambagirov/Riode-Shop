using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.Domain.Models.Entities
{
    public class SpesificationValues : BaseEntity
    {
        public int SpesificationId { get; set; }
        public virtual Spesification Spesification { get; set; }
        public int ProductId { get; set; }
        public virtual Products Product { get; set; }
        public string Value { get; set; }
    }
}
