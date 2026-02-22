using Microsoft.EntityFrameworkCore;
using Snapcart.Domain.Entities;

namespace Snapcart.Infrastructure.Data
{
    public class SnapcartDbContext : DbContext
    {
        public SnapcartDbContext(DbContextOptions<SnapcartDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Buy> Buys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User 1 → N List
            modelBuilder.Entity<List>()
                .HasOne(l => l.User)
                .WithMany(u => u.Lists)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // List 1 → N Product
            modelBuilder.Entity<Product>()
                .HasOne(p => p.List)
                .WithMany(l => l.Products)
                .HasForeignKey(p => p.ListId)
                .OnDelete(DeleteBehavior.Cascade);

            // User 1 → N Buy
            modelBuilder.Entity<Buy>()
                .HasOne(b => b.User)
                .WithMany(u => u.Buys)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // List 1 → 1 Buy
            modelBuilder.Entity<Buy>()
                .HasOne(b => b.List)
                .WithMany()
                .HasForeignKey(b => b.ListId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
