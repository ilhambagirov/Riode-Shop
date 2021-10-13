using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Riode.Application.FaqModule
{
    public class FaqViewModel
    {
        public int? Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
