using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.Entities;
using System;

namespace Riode.WebUI.Models.DataContext
{
    public class RiodeDBContext : DbContext
    {

        public Guid InstanceNumber { get; set; }
        public RiodeDBContext(DbContextOptions options)
            : base(options)
        {
        }

       

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=Riode;User Id=sa;Password=query");
            }
        }

        public DbSet<Category> Category { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<BlogImages> BlogImages { get; set; }

        public DbSet<Brands> Brands { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Colors> Colors { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Images> Images { get; set; }
    }
}
