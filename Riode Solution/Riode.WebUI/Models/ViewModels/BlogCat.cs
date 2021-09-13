using System;
using System.Collections.Generic;
using System.Linq;
using Riode.WebUI.Models.Entities;

namespace Riode.WebUI.Models.ViewModels
{
    public class BlogCat
    {
        public List<Category> BlogCats { get; set; }
        public Blog Blog { get; set; }

        public List<Blog> Blogs { get; set; }
    }
}
