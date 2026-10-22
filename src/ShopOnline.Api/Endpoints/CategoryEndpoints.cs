using Microsoft.EntityFrameworkCore;
using ShopOnline.Application.Dto;
using ShopOnline.Application.Mapping;
using ShopOnline.Domain.Entities;
using ShopOnline.Infrastructure.Data;

namespace ShopOnline.Api.Endpoints;

internal static class CategoryEndpoints
{
    const string GetCategoryEndpointName = "GetCategory";

    internal static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/categories");

        group.MapGet("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            var category = await context.Categories.FindAsync(id);

            return category is null ? Results.NotFound() : Results.Ok(category.ToDto());
        })
        .WithName(GetCategoryEndpointName);

        group.MapGet("/", async (ShopDatabaseContext context)
            => await context.Categories
                .AsNoTracking()
                .Select(category => category.ToDto())
                .ToListAsync());

        group.MapDelete("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            await context.Categories
                .Where(cat => cat.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        group.MapPost("/", async (CreateCategoryDto dto, ShopDatabaseContext context) =>
        {
            Category cat = new()
            {
                Name = dto.Name
            };

            context.Categories.Add(cat);
            await context.SaveChangesAsync();

            CategoryDto catDto = cat.ToDto();

            return Results.CreatedAtRoute(GetCategoryEndpointName, new { id = catDto.Id } , catDto);
        });

        group.MapPut("/{id}", async (int id, UpdateCategoryDto dto, ShopDatabaseContext context) =>
        {
            var category = await context.Categories.FindAsync(id);

            if (category is null)
                return Results.NotFound();

            category.Name = dto.Name;
            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
