using Microsoft.EntityFrameworkCore;
using ShopOnline.Application.Dto;
using ShopOnline.Application.Entities;
using ShopOnline.Application.Mapping;
using ShopOnline.Infrastructure.Data;

namespace ShopOnline.Api.Endpoints;

internal static class CollectionEndpoints
{
    const string GetCollectionEndpointName = "GetCollection";

    internal static void MapCollectionEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/collections");

        group.MapGet("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            var collection = await context.Collections.FindAsync(id);

            return collection is null ? Results.NotFound() : Results.Ok(collection.ToDto());
        })
        .WithName(GetCollectionEndpointName);

        group.MapGet("/", async (ShopDatabaseContext context)
            => await context.Collections
                .AsNoTracking()
                .Select(collection => collection.ToDto())
                .ToListAsync());

        group.MapDelete("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            await context.Collections
                .Where(collection => collection.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        group.MapPost("/", async (CreateCollectionDto dto, ShopDatabaseContext context) =>
        {
            Collection collection = new()
            {
                Name = dto.Name
            };

            context.Collections.Add(collection);
            await context.SaveChangesAsync();

            CollectionDto collectionDto = collection.ToDto();

            return Results.CreatedAtRoute(GetCollectionEndpointName, new { id = collectionDto.Id }, collectionDto);
        });

        group.MapPut("/{id}", async (int id, UpdateCollectionDto dto, ShopDatabaseContext context) =>
        {
            var collection = await context.Collections.FindAsync(id);

            if (collection is null)
                return Results.NotFound();

            collection.Name = dto.Name;
            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
