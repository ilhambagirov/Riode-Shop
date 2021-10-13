using System;
using System.Collections.Generic;
using System.Linq;
using Riode.Domain.Models.Entities;

namespace Riode.Domain.Models.ViewModels
{
    public class BlogCat
    {
        public List<Category> BlogCats { get; set; }
        public Blog Blog { get; set; }

        public List<Blog> Blogs { get; set; }
    }
}
