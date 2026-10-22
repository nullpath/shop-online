using Microsoft.EntityFrameworkCore;
using ShopOnline.Domain.Entities;

namespace ShopOnline.Infrastructure.Data;

public static class SeedDataExtensions
{
    public static void InitializeData(this DbContext context)
    {
        if (context.Set<Category>().Any())
        {
            return;
        }

        var data = new SeedData();

        context.Set<Gender>().AddRange(data.Genders);
        context.Set<Color>().AddRange(data.Colors);
        context.Set<Collection>().AddRange(data.Collections);
        context.Set<Category>().AddRange(data.Categories);
        context.Set<Product>().AddRange(data.Products);

        context.SaveChanges();
    }
}
