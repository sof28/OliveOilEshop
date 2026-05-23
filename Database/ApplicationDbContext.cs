using Microsoft.EntityFrameworkCore;
using OliveOilEshop.Entities;

namespace OliveOilEshop.Database;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<User> Users { get; set; }

}