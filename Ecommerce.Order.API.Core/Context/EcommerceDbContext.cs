using Ecommerce.Order.API.Core.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Order.API.Core.Context
{
    public class EcommerceDbContext : DbContext
    {
        #region Constructor
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {
        }
        #endregion

        #region DBSets
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<OrderDetailModel> OrderDetails { get; set; }
        #endregion

        #region OnModelCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
               .Entity<ProductModel>()
               .ToTable("Products");

            modelBuilder
                .Entity<OrderModel>()
                .ToTable("Orders");

            modelBuilder
                .Entity<OrderDetailModel>()
                .ToTable("OrderDetails")
                .HasKey(d => new { d.Id });

            modelBuilder
                .Entity<OrderDetailModel>()
                .HasOne(d => d.Order)
                .WithMany(d => d.OrderDetails)
                .HasForeignKey(d => d.OrderId);

            modelBuilder
                .Entity<OrderDetailModel>()
                .HasOne(d => d.Product)
                .WithMany(d => d.OrderDetails)
                .HasForeignKey(d => d.ProductId);

            base.OnModelCreating(modelBuilder);
        } 
        #endregion
    }
}
