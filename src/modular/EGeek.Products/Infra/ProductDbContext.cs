using EGeek.Products.Domain;
using Microsoft.EntityFrameworkCore;

namespace EGeek.Products.Infra;

internal class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Product>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(120);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.CreatedBy).HasMaxLength(120);
            entity.Property(e => e.UpdatedBy).HasMaxLength(120);
        });

        builder.HasDefaultSchema("products");
    }
}