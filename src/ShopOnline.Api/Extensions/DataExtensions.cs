using Microsoft.EntityFrameworkCore;
using ShopOnline.Infrastructure.Data;

namespace ShopOnline.Api.Extensions;

internal static class DataExtensions
{
    internal static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ShopDatabaseContext>();

        dbContext.Database.Migrate();
    }

    internal static void InitializeDb(this WebApplicationBuilder builder)
    {
        string databaseConnectionString = builder.Configuration.GetConnectionString("ShopDatabase")
                                          ?? throw new InvalidOperationException("Database connection string is null 'ShopDatabase'.");

        builder.Services.AddSqlite<ShopDatabaseContext>(
            databaseConnectionString,
            optionsAction: optionsBuilder => optionsBuilder.UseSeeding((context, _) => context.InitializeData()));
    }
}
