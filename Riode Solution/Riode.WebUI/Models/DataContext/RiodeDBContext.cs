using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.Entities;
using System;

namespace Riode.WebUI.Models.DataContext
{
    public class RiodeDBContext : DbContext
    {
        public RiodeDBContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Brands> Brands { get; set; }
        public DbSet<Size> Size { get; set; }
        public DbSet<Colors> Colors { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Images> Images { get; set; }
        public DbSet<ProductSizeColorItem> ProductSizeColorCollection { get; set; }
        public DbSet<FAQ> FAQs { get; set; }
        public DbSet<Contact> ContactPosts { get; set; }
        public DbSet<Spesification> Spesifications { get; set; }
        public DbSet<SpesificationCategoryItem> SpesificationCategoryCollection { get; set; }
        public DbSet<SpesificationValues> SpesificationValues { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
  
    }
}
