using ShopOnline.Api.Extensions;
using ShopOnline.Application.Dto;
using ShopOnline.Application.Services;

namespace ShopOnline.Api.Endpoints;

internal static class ProductEndpoints
{
    const string GetProductEndpointName = "GetProduct";

    internal static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/products");

        group.MapGet("/{id}", async (int id, IProductService productService) =>
        {
            var result = await productService.GetProductAsync(id);

            return result.Match(
                product => Results.Ok(product),
                errors => errors.ToProblem());
        })
        .WithName(GetProductEndpointName);

        group.MapGet("/", async ([AsParameters] ProductFilter filter, IProductService productService)
            => await productService.GetProductsAsync(filter));

        group.MapDelete("/{id}", async (int id, IProductService productService) =>
        {
            var result = await productService.DeleteProductAsync(id);

            return result.Match(
                _ => Results.NoContent(),
                errors => errors.ToProblem());
        });

        group.MapPost("/", async (CreateProductDto dto, IProductService productService) =>
        {
            var result = await productService.CreateProductAsync(dto);

            return result.Match(
                product => Results.CreatedAtRoute(GetProductEndpointName, new { id = product.Id }, product),
                errors => errors.ToProblem());
        });

        group.MapPut("/{id}", async (int id, UpdateProductDto dto, IProductService productService) =>
        {
            var result = await productService.UpdateProductAsync(id, dto);

            return result.Match(
                _ => Results.NoContent(),
                errors => errors.ToProblem());
        });
    }
}
