using Microsoft.EntityFrameworkCore;
using ShopOnline.Application.Dto;
using ShopOnline.Application.Entities;
using ShopOnline.Application.Mapping;
using ShopOnline.Infrastructure.Data;

namespace ShopOnline.Api.Endpoints;

internal static class SizeEndpoints
{
    const string GetSizeEndpointName = "GetSize";

    internal static void MapSizeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/sizes");

        group.MapGet("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            var size = await context.Sizes.FindAsync(id);

            return size is null ? Results.NotFound() : Results.Ok(size.ToDto());
        })
        .WithName(GetSizeEndpointName);

        group.MapGet("/", async (ShopDatabaseContext context)
            => await context.Sizes
                .AsNoTracking()
                .Select(size => size.ToDto())
                .ToListAsync());

        group.MapDelete("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            await context.Sizes
                .Where(size => size.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        group.MapPost("/", async (CreateSizeDto dto, ShopDatabaseContext context) =>
        {
            Size size = new()
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name
            };

            context.Sizes.Add(size);
            await context.SaveChangesAsync();

            SizeDto sizeDto = size.ToDto();

            return Results.CreatedAtRoute(GetSizeEndpointName, new { id = sizeDto.Id }, sizeDto);
        });

        group.MapPut("/{id}", async (int id, UpdateSizeDto dto, ShopDatabaseContext context) =>
        {
            var size = await context.Sizes.FindAsync(id);

            if (size is null)
                return Results.NotFound();

            size.CategoryId = dto.CategoryId;
            size.Name = dto.Name;
            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
