using Microsoft.EntityFrameworkCore;
using ShopOnline.Application.Dto;
using ShopOnline.Application.Mapping;
using ShopOnline.Domain.Entities;
using ShopOnline.Infrastructure.Data;

namespace ShopOnline.Api.Endpoints;

internal static class ColorEndpoints
{
    const string GetColorEndpointName = "GetColor";

    internal static void MapColorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/colors");

        group.MapGet("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            var color = await context.Colors.FindAsync(id);

            return color is null ? Results.NotFound() : Results.Ok(color.ToDto());
        })
        .WithName(GetColorEndpointName);

        group.MapGet("/", async (ShopDatabaseContext context)
            => await context.Colors
                .AsNoTracking()
                .Select(color => color.ToDto())
                .ToListAsync());

        group.MapDelete("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            await context.Colors
                .Where(color => color.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        group.MapPost("/", async (CreateColorDto dto, ShopDatabaseContext context) =>
        {
            Color color = new()
            {
                Name = dto.Name,
                HexCode = dto.HexCode
            };

            context.Colors.Add(color);
            await context.SaveChangesAsync();

            ColorDto colorDto = color.ToDto();

            return Results.CreatedAtRoute(GetColorEndpointName, new { id = colorDto.Id }, colorDto);
        });

        group.MapPut("/{id}", async (int id, UpdateColorDto dto, ShopDatabaseContext context) =>
        {
            var color = await context.Colors.FindAsync(id);

            if (color is null)
                return Results.NotFound();

            color.Name = dto.Name;
            color.HexCode = dto.HexCode;
            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
