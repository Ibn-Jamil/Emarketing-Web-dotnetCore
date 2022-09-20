using AngularCoreEmarketing.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AngularCoreEmarketing.Data
{
    public class ApplicationDbContext :IdentityDbContext<ApplicationUser>
    {
        public DbSet<CustomerReview> CustomerReviews { get; set; }
        public DbSet<ProductDetails> ProductsDetails { get; set; }
        public DbSet<ProductFeedback> ProductsFeedback { get; set; }
        public DbSet<CatagoryMajor> CatagoriesMajor { get; set; }
        public DbSet<CatagoryMajorSub> CatagoriesMajorSub { get; set; }
        public DbSet<CatagorySpecific> CatagoriesSpecific { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<DeliveryAddress> DeliveryAddress { get; set; }
        public DbSet<OrderdProduct> OrderdProducts { get; set; }
        public DbSet<ImagesDirectory> ImagesDirectories { get; set; }
        public DbSet<ProductSizes> ProductsSizes { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
            
        }
    }
}
