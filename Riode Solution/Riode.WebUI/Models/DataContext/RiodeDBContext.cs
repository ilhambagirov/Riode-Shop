using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Riode.WebUI.Models.Entities;
using Riode.WebUI.Models.Entities.Membership;

namespace Riode.WebUI.Models.DataContext
{
    public class RiodeDBContext : IdentityDbContext<RiodeUser, RiodeRole, int, RiodeUserClaim, RiodeUserRole, RiodeUserLogin, RiodeRoleClaim, RiodeUserToken>
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RiodeUser>(e =>
            {
                e.ToTable("Users", "Membership");
            });

            builder.Entity<RiodeRole>(e =>
            {
                e.ToTable("Roles", "Membership");
            });

            builder.Entity<RiodeUserLogin>(e =>
            {
                e.ToTable("UserLogins", "Membership");
            });

            builder.Entity<RiodeUserToken>(e =>
            {
                e.ToTable("UserTokens", "Membership");
            });

            builder.Entity<RiodeUserClaim>(e =>
            {
                e.ToTable("UserClaims", "Membership");
            });

            builder.Entity<RiodeRoleClaim>(e =>
            {
                e.ToTable("RoleClaims", "Membership");
            });

            builder.Entity<RiodeUserRole>(e =>
            {
                e.ToTable("UserRoles", "Membership");
            });


        }

    }
}
