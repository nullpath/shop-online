using Microsoft.EntityFrameworkCore;
using ShopOnline.Application.Dto;
using ShopOnline.Application.Entities;
using ShopOnline.Application.Mapping;
using ShopOnline.Infrastructure.Data;

namespace ShopOnline.Api.Endpoints;

internal static class ProductEndpoints
{
    const string GetProductEndpointName = "GetProduct";

    internal static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/products");

        group.MapGet("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            var product = await context.Products
                .Include(product => product.Sizes)
                .AsNoTracking()
                .FirstOrDefaultAsync(product => product.Id == id);

            return product is null ? Results.NotFound() : Results.Ok(product.ToDto());
        })
        .WithName(GetProductEndpointName);

        group.MapGet("/", async (ShopDatabaseContext context)
            => await context.Products
                .Include(product => product.Sizes)
                .AsNoTracking()
                .Select(product => product.ToDto())
                .ToListAsync());

        group.MapDelete("/{id}", async (int id, ShopDatabaseContext context) =>
        {
            await context.Products
                .Where(product => product.Id == id)
                .ExecuteDeleteAsync();

            return Results.NoContent();
        });

        group.MapPost("/", async (CreateProductDto dto, ShopDatabaseContext context) =>
        {
            Product product = new()
            {
                Name = dto.Name,
                Price = dto.Price,
                SubcategoryId = dto.SubcategoryId,
                ColorId = dto.ColorId,
                CollectionId = dto.CollectionId,
                GenderId = dto.GenderId,
                Sizes = dto.Sizes
                    .Select(size => new ProductSize { SizeId = size.SizeId, Stock = size.Stock })
                    .ToList()
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            ProductDto productDto = product.ToDto();

            return Results.CreatedAtRoute(GetProductEndpointName, new { id = productDto.Id }, productDto);
        });

        group.MapPut("/{id}", async (int id, UpdateProductDto dto, ShopDatabaseContext context) =>
        {
            var product = await context.Products
                .Include(product => product.Sizes)
                .FirstOrDefaultAsync(product => product.Id == id);

            if (product is null)
                return Results.NotFound();

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.SubcategoryId = dto.SubcategoryId;
            product.ColorId = dto.ColorId;
            product.CollectionId = dto.CollectionId;
            product.GenderId = dto.GenderId;

            product.Sizes.Clear();
            product.Sizes.AddRange(dto.Sizes
                .Select(size => new ProductSize { SizeId = size.SizeId, Stock = size.Stock }));

            await context.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}
