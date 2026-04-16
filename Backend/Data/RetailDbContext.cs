using Microsoft.EntityFrameworkCore;
using RetailApi.Models;

namespace RetailApi.Data
{
    public class RetailDbContext : DbContext
    {
        public RetailDbContext(DbContextOptions<RetailDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Relationships
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<CartItem>()
                .HasKey(c => c.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductId);

            // Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, Name = "Margherita Pizza", Category = "Pizza", Price = 12.99m, Stock = 50, ImageUrl = "https://images.unsplash.com/photo-1604382354936-07c5d9983bd3?w=500" },
                new Product { ProductId = 2, Name = "Pepperoni Pizza", Category = "Pizza", Price = 14.99m, Stock = 40, ImageUrl = "https://images.unsplash.com/photo-1628840042765-356cda07504e?w=500" },
                new Product { ProductId = 3, Name = "Veggie Pizza", Category = "Pizza", Price = 13.50m, Stock = 30, ImageUrl = "https://images.unsplash.com/photo-1571407970349-bc81e7e96d47?w=500" },
                new Product { ProductId = 4, Name = "Coca Cola", Category = "Cold Drinks", Price = 2.50m, Stock = 100, ImageUrl = "https://images.unsplash.com/photo-1622483767028-3f66f32aef97?w=500" },
                new Product { ProductId = 5, Name = "Pepsi", Category = "Cold Drinks", Price = 2.40m, Stock = 100, ImageUrl = "https://images.unsplash.com/photo-1553456533-535316cb60d7?w=500" },
                new Product { ProductId = 6, Name = "Sparkling Water", Category = "Cold Drinks", Price = 1.80m, Stock = 80, ImageUrl = "https://images.unsplash.com/photo-1548964856-ac56f17e3e7f?w=500" },
                new Product { ProductId = 7, Name = "Garlic Bread", Category = "Breads", Price = 4.99m, Stock = 60, ImageUrl = "https://images.unsplash.com/photo-1573140247632-f8fd74997d5c?w=500" },
                new Product { ProductId = 8, Name = "Cheese Bread", Category = "Breads", Price = 5.99m, Stock = 50, ImageUrl = "https://images.unsplash.com/photo-1589187151003-0dd33123ff5b?w=500" }
            );

            // Seed Admin User
            // Note: Password is 'admin123'
            modelBuilder.Entity<User>().HasData(
                new User { UserId = 1, Username = "admin", Email = "admin@retail.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), Role = "Admin" }
            );
        }
    }
}
