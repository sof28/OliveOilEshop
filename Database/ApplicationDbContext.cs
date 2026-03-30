using Microsoft.EntityFrameworkCore;
using OliveOilEshop.Entities;

namespace OliveOilEshop.Database;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Order> OrderItems { get; set; }
    public DbSet<Order> Products { get; set; }
    public DbSet<Order> Users { get; set; }

}