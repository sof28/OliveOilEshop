using Microsoft.EntityFrameworkCore;
using ModularCommerce.Entities;

namespace ModularCommerce.Database;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<ProductAttributeDefinition> ProductAttributeDefinitions { get; set; }
    public DbSet<ProductVariant> ProductVariants { get; set; }
    public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.UnitPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ProductVariant>()
            .Property(pv => pv.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<ProductCategory>()
            .HasIndex(pc => pc.Slug)
            .IsUnique();

        modelBuilder.Entity<ProductType>()
            .HasIndex(pt => pt.Code)
            .IsUnique();

        modelBuilder.Entity<ProductAttributeDefinition>()
            .HasIndex(ad => new { ad.ProductTypeId, ad.Key })
            .IsUnique();

        modelBuilder.Entity<ProductVariant>()
            .HasIndex(pv => pv.Sku)
            .IsUnique();

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.ProductType)
            .WithMany(pt => pt.Products)
            .HasForeignKey(p => p.ProductTypeId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.ParentCategory)
            .WithMany(pc => pc.ChildCategories)
            .HasForeignKey(pc => pc.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
