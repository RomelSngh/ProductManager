using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProductManagement.Models;
using System.Reflection.Emit;

namespace ProductManagement.Data
{
    public class ProductDbContext : IdentityDbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.HasSequence<int>("Product_Sequence", schema: "dbo")
                .StartsAt(1)
                .IncrementsBy(1);

            var dtPart = DateTime.UtcNow.ToString("yyyyyMM");

            builder.Entity<Product>()
                .Property(p => p.ProductCode)
                .HasDefaultValue(string.Format("CONCAT('{0}', CAST(NEXT VALUE FOR dbo.Product_Sequence AS VARCHAR))", dtPart));

            builder.Entity<Product>()
                .HasOne(i => i.ProductCategory)
                .WithMany(s => s.Products)
                .HasForeignKey(i => i.CategoryId); // Assuming SiteId is the foreign key in Incident

            builder.Entity<Category>().HasData(
                new Category { CategoryCode= "ABC123" ,CategoryId=1, IsActive=true , Name = "TestCategory"  }
                );
        }
    }
}