using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.Domain.Models.Entities
{
    public class AuditLog : BaseEntity
    {
        public string Path { get; set; }
        public bool IsHttps { get; set; }
        public string QueryString { get; set; }
        public string Method { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int StatuCode { get; set; }
        public DateTime RequestTime { get; set; }
        public DateTime ResponseTime { get; set; }
    }

}
