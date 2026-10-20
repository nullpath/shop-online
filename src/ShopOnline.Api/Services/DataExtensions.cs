using Microsoft.EntityFrameworkCore;
using ShopOnline.Application.Entities;
using ShopOnline.Infrastructure.Data;

namespace ShopOnline.Api.Services;

internal static class DataExtensions
{
    internal static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ShopDatabaseContext>();
        
        dbContext.Database.Migrate();
    }

    internal static void InitializeDatabase(this WebApplicationBuilder builder)
    {
        string databaseConnectionString = builder.Configuration.GetConnectionString("ShopDatabase")
                                          ?? throw new InvalidOperationException("Database connection string is null 'ShopDatabase'.");
        
        builder.Services.AddSqlite<ShopDatabaseContext>(
            databaseConnectionString,
            optionsAction: optionsBuilder => optionsBuilder.UseSeeding((context, _) => context.InitializeData()));
    }

    static void InitializeData(this DbContext context)
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