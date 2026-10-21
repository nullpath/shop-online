using Microsoft.EntityFrameworkCore;
using ShopOnline.Application.Dto;
using ShopOnline.Application.Entities;
using ShopOnline.Application.Mapping;
using ShopOnline.Infrastructure.Data;

namespace ShopOnline.Api.Endpoints;

internal static class SubcategoryEndpoints
{
    const string GetSubcategoryEndpointName = "GetSubcategory";

    internal static void MapSubcategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/subcategories");

        group.MapGet("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            var subcategory = await context.Subcategories.FindAsync(id);

            return subcategory is null ? Results.NotFound() : Results.Ok(subcategory.ToDto());
        })
        .WithName(GetSubcategoryEndpointName);

        group.MapGet("/", async (ShopDatabaseContext context)
            => await context.Subcategories
                .AsNoTracking()
                .Select(subcategory => subcategory.ToDto())
                .ToListAsync());

        group.MapDelete("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            await context.Subcategories
                .Where(subcategory => subcategory.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        group.MapPost("/", async (CreateSubcategoryDto dto, ShopDatabaseContext context) =>
        {
            Subcategory subcategory = new()
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name
            };

            context.Subcategories.Add(subcategory);
            await context.SaveChangesAsync();

            SubcategoryDto subcategoryDto = subcategory.ToDto();

            return Results.CreatedAtRoute(GetSubcategoryEndpointName, new { id = subcategoryDto.Id }, subcategoryDto);
        });

        group.MapPut("/{id}", async (int id, UpdateSubcategoryDto dto, ShopDatabaseContext context) =>
        {
            var subcategory = await context.Subcategories.FindAsync(id);

            if (subcategory is null)
                return Results.NotFound();

            subcategory.CategoryId = dto.CategoryId;
            subcategory.Name = dto.Name;
            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
