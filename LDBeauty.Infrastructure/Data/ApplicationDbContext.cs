using LDBeauty.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LDBeauty.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Email)
                .IsUnique();

            builder.Entity<UserImage>()
                .HasKey(x => new { x.ImageId, x.ApplicationUserId });

            builder.Entity<UserProduct>()
                .HasKey(x => new { x.ProductId, x.ApplicationUserId });
        }

        public DbSet<AddedProduct> AddedProducts { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<ImgCategory> ImgCategories { get; set; }

        public DbSet<Make> Makes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public DbSet<UserImage> UsersImages { get; set; }

        public DbSet<UserProduct> UsersProducts { get; set; }
    }
}