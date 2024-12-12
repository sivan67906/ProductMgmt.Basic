using Microsoft.EntityFrameworkCore;
using ProductMgmt.Domain.Product;

namespace ProductMgmt.Persistence.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{

    public DbSet<Product> Products { get; set; }

    // Override OnModelCreating to configure the entity model
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Product entity
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(p => p.Id); // Set primary key

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(100); // Limit name length

            entity.Property(p => p.Price)
                  .HasColumnType("decimal(18,2)"); // Define precision for the Price column

            entity.Property(p => p.Description)
                  .HasMaxLength(1000); // Limit description length

            entity.Property(p => p.SKU)
                  .IsRequired()
                  .HasMaxLength(50); // SKU is required and limited to 50 characters
        });
    }
}