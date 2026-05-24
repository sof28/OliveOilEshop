using OliveOilEshop.Database;
using OliveOilEshop.Entities;

namespace OliveOilEshop.Extensions;

public static class DataSeederExtension
{
    public static async Task SeedProductsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Name = "Extra Virgin Olive Oil",
                    Description = "Cold-pressed from hand-picked Koroneiki olives. Rich, fruity flavor with a peppery finish. Ideal for salads and dipping.",
                    Price = 18.99m,
                    ImageUrl = "assets/images/olive-oil-1.jpg"
                },
                new Product
                {
                    Name = "Organic Kalamata Olive Oil",
                    Description = "Single-origin from the Kalamata region of Greece. Smooth, buttery texture with hints of almonds. Perfect for gourmet cooking.",
                    Price = 24.99m,
                    ImageUrl = "assets/images/olive-oil-2.jpg"
                },
                new Product
                {
                    Name = "Premium Blend Olive Oil",
                    Description = "A carefully crafted blend of early and late harvest olives. Versatile and full-bodied — perfect for both cooking and dipping.",
                    Price = 14.99m,
                    ImageUrl = "assets/images/olive-oil-3.jpg"
                }
            );
            await context.SaveChangesAsync();
        }
    }
}

