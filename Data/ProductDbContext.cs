using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ProductManagement.Models;
using System;
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

            //var dtPart = DateTime.UtcNow.ToString("yyyyyMM");

            builder.Entity<Product>()
           .Property(b => b.CreatedDate)
           .HasDefaultValueSql("getdate()");

            builder.Entity<Category>()
            .Property(b => b.CreatedDate)
            .HasDefaultValueSql("getdate()");

            builder.Entity<Product>()
                .HasOne(i => i.ProductCategory)
                .WithMany(s => s.Products)
                .HasForeignKey(i => i.CategoryId); // Assuming SiteId is the foreign key in Incident

            //builder.Entity<Product>()
            //   .Property(p => p.ProductCode)
            //   .HasDefaultValue(string.Format("CONCAT('{0}', CAST(NEXT VALUE FOR dbo.Product_Sequence AS VARCHAR))", dtPart));

            builder.Entity<Category>().HasData(
                new Category { CategoryCode= "ABC123" ,CategoryId=1, IsActive=true , Name = "TestCategory"  }
                );
        }

        public string GetNextSequenceValue()
        {
            var sqlCommandText =
                (@"DECLARE @result Varchar(50)
	                        DECLARE @next int 
	                        SET @next = CAST(NEXT VALUE FOR dbo.Product_Sequence AS VARCHAR)
	
	                        SELECT  @result= 
	                                 CONCAT(CONVERT(Varchar(6),GetDate(),112)+'-',FORMAT(@next, '000'))

	                        SELECT @result");


            var connection = this.Database.GetDbConnection();
            connection.Open();
            using var cmd = connection.CreateCommand();
            cmd.CommandText = sqlCommandText;
            var obj = cmd.ExecuteScalar();
            connection.Close();
            var seqnum = obj.ToString();
            
            return seqnum;
        }



    }
}