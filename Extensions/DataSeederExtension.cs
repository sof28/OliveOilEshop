using ModularCommerce.Database;
using ModularCommerce.Entities;

namespace ModularCommerce.Extensions;

public static class DataSeederExtension
{
    public static async Task SeedProductsAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        if (!context.Products.Any())
        {
            var oliveOilType = new ProductType
            {
                Name = "Olive Oil",
                Code = "olive-oil",
                Description = "Oil-based consumable products"
            };

            var oilsCategory = new ProductCategory
            {
                Name = "Olive Oils",
                Slug = "olive-oils",
                Description = "Premium olive oil collection"
            };

            context.ProductTypes.Add(oliveOilType);
            context.ProductCategories.Add(oilsCategory);
            await context.SaveChangesAsync();

            context.Products.AddRange(
                new Product
                {
                    Name = "Extra Virgin Olive Oil",
                    Slug = "extra-virgin-olive-oil",
                    Description = "Cold-pressed from hand-picked Koroneiki olives. Rich, fruity flavor with a peppery finish. Ideal for salads and dipping.",
                    Price = 18.99m,
                    ImageUrl = "assets/images/olive-oil-1.jpg",
                    Sku = "OO-EVOO-001",
                    StockQuantity = 100,
                    CategoryId = oilsCategory.Id,
                    ProductTypeId = oliveOilType.Id
                },
                new Product
                {
                    Name = "Organic Kalamata Olive Oil",
                    Slug = "organic-kalamata-olive-oil",
                    Description = "Single-origin from the Kalamata region of Greece. Smooth, buttery texture with hints of almonds. Perfect for gourmet cooking.",
                    Price = 24.99m,
                    ImageUrl = "assets/images/olive-oil-2.jpg",
                    Sku = "OO-ORG-002",
                    StockQuantity = 100,
                    CategoryId = oilsCategory.Id,
                    ProductTypeId = oliveOilType.Id
                },
                new Product
                {
                    Name = "Premium Blend Olive Oil",
                    Slug = "premium-blend-olive-oil",
                    Description = "A carefully crafted blend of early and late harvest olives. Versatile and full-bodied — perfect for both cooking and dipping.",
                    Price = 14.99m,
                    ImageUrl = "assets/images/olive-oil-3.jpg",
                    Sku = "OO-PRM-003",
                    StockQuantity = 100,
                    CategoryId = oilsCategory.Id,
                    ProductTypeId = oliveOilType.Id
                }
            );
            await context.SaveChangesAsync();
        }
    }
}

