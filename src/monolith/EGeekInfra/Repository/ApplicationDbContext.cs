using EGeekDomain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EGeekinfra.Repository;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Stock> Stocks { get; set; }
    // public DbSet<Order> Orders { get; set; }
    // public DbSet<OrderItem> Items { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(120);
            entity.Property(e => e.Description).HasMaxLength(500);
        });
        
        modelBuilder.Entity<Stock>(entity =>
        {
            entity.Property(e => e.Color).HasMaxLength(12);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CreditCardMasked).HasMaxLength(16);
        });
    }
}