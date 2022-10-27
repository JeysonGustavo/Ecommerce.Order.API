using Ecommerce.Order.API.Core.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Order.API.Core.Context
{
    public class EcommerceDbContext : DbContext
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {
        }

        public DbSet<OrderModel> Orders { get; set; }
        //public DbSet<ProductModel> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<OrderModel>()
                .ToTable("Orders");

            modelBuilder
                .Entity<ProductModel>()
                .ToTable("Products");

            modelBuilder
                .Entity<OrderModel>()
                .HasMany(o => o.Products)
                .WithMany(c => c.Orders)
                .UsingEntity(j => j.ToTable("ProductOrder"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
