using ErrorOr;
using Microsoft.EntityFrameworkCore;
using ShopOnline.Application.Dto;
using ShopOnline.Application.Mapping;
using ShopOnline.Application.Services;
using ShopOnline.Domain.Entities;
using ShopOnline.Infrastructure.Data;

namespace ShopOnline.Infrastructure.Services;

internal class ProductService(ShopDatabaseContext context) : IProductService
{
    public async Task<List<ProductDto>> GetProductsAsync(ProductFilter filter)
    {
        IQueryable<Product> query = context.Products
            .Include(product => product.Sizes)
            .AsNoTracking();

        if (filter.CategoryId is int categoryId)
            query = query.Where(product => product.Subcategory.CategoryId == categoryId);

        if (filter.SubcategoryId is int subcategoryId)
            query = query.Where(product => product.SubcategoryId == subcategoryId);

        if (filter.SizeId is int sizeId)
            query = query.Where(product => product.Sizes.Any(size => size.SizeId == sizeId && size.Stock > 0));

        if (filter.ColorId is int colorId)
            query = query.Where(product => product.ColorId == colorId);

        if (filter.CollectionId is int collectionId)
            query = query.Where(product => product.CollectionId == collectionId);

        if (filter.GenderId is int genderId)
            query = query.Where(product => product.GenderId == genderId);

        if (filter.MinPrice is decimal minPrice)
            query = query.Where(product => product.Price >= minPrice);

        if (filter.MaxPrice is decimal maxPrice)
            query = query.Where(product => product.Price <= maxPrice);

        query = filter.SortBy switch
        {
            ProductSort.PriceAsc => query.OrderBy(product => product.Price),
            ProductSort.PriceDesc => query.OrderByDescending(product => product.Price),
            ProductSort.NameAsc => query.OrderBy(product => product.Name),
            ProductSort.NameDesc => query.OrderByDescending(product => product.Name),
            _ => query.OrderBy(product => product.Id)
        };

        var products = await query.ToListAsync();

        return products.Select(product => product.ToDto()).ToList();
    }

    public async Task<ErrorOr<ProductDto>> GetProductAsync(int id)
    {
        var product = await context.Products
            .Include(product => product.Sizes)
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id);

        if (product is null)
            return Error.NotFound(description: $"Product {id} does not exist.");

        return product.ToDto();
    }

    public async Task<ErrorOr<ProductDto>> CreateProductAsync(CreateProductDto dto)
    {
        var errors = await ValidateReferencesAsync(
            dto.SubcategoryId, dto.ColorId, dto.CollectionId, dto.GenderId, dto.Sizes);

        if (errors.Count > 0)
            return errors;

        Product product = new()
        {
            Name = dto.Name,
            Price = dto.Price,
            SubcategoryId = dto.SubcategoryId,
            ColorId = dto.ColorId,
            CollectionId = dto.CollectionId,
            GenderId = dto.GenderId,
            Sizes = ToProductSizes(dto.Sizes)
        };

        context.Products.Add(product);
        await context.SaveChangesAsync();

        return product.ToDto();
    }

    public async Task<ErrorOr<Updated>> UpdateProductAsync(int id, UpdateProductDto dto)
    {
        var product = await context.Products
            .Include(product => product.Sizes)
            .FirstOrDefaultAsync(product => product.Id == id);

        if (product is null)
            return Error.NotFound(description: $"Product {id} does not exist.");

        var errors = await ValidateReferencesAsync(
            dto.SubcategoryId, dto.ColorId, dto.CollectionId, dto.GenderId, dto.Sizes);

        if (errors.Count > 0)
            return errors;

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.SubcategoryId = dto.SubcategoryId;
        product.ColorId = dto.ColorId;
        product.CollectionId = dto.CollectionId;
        product.GenderId = dto.GenderId;

        product.Sizes.Clear();
        product.Sizes.AddRange(ToProductSizes(dto.Sizes));

        await context.SaveChangesAsync();

        return Result.Updated;
    }

    public async Task<ErrorOr<Deleted>> DeleteProductAsync(int id)
    {
        var deletedCount = await context.Products
            .Where(product => product.Id == id)
            .ExecuteDeleteAsync();

        if (deletedCount == 0)
            return Error.NotFound(description: $"Product {id} does not exist.");

        return Result.Deleted;
    }

    async Task<List<Error>> ValidateReferencesAsync(
        int subcategoryId, int colorId, int? collectionId, int genderId, List<ProductSizeDto> sizes)
    {
        List<Error> errors = [];

        var categoryId = await context.Subcategories
            .Where(subcategory => subcategory.Id == subcategoryId)
            .Select(subcategory => (int?)subcategory.CategoryId)
            .FirstOrDefaultAsync();

        if (categoryId is null)
            errors.Add(Error.Validation(nameof(ProductDto.SubcategoryId), $"Subcategory {subcategoryId} does not exist."));

        if (!await context.Colors.AnyAsync(color => color.Id == colorId))
            errors.Add(Error.Validation(nameof(ProductDto.ColorId), $"Color {colorId} does not exist."));

        if (collectionId is not null && !await context.Collections.AnyAsync(collection => collection.Id == collectionId))
            errors.Add(Error.Validation(nameof(ProductDto.CollectionId), $"Collection {collectionId} does not exist."));

        if (!await context.Genders.AnyAsync(gender => gender.Id == genderId))
            errors.Add(Error.Validation(nameof(ProductDto.GenderId), $"Gender {genderId} does not exist."));

        var sizeIds = sizes.Select(size => size.SizeId).ToList();

        var duplicatedSizeIds = sizeIds
            .GroupBy(sizeId => sizeId)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicatedSizeIds.Count > 0)
            errors.Add(Error.Validation(nameof(ProductDto.Sizes), $"Sizes listed more than once: {string.Join(", ", duplicatedSizeIds)}."));

        if (categoryId is not null && sizeIds.Count > 0)
        {
            var validSizeIds = await context.Sizes
                .Where(size => sizeIds.Contains(size.Id) && size.CategoryId == categoryId)
                .Select(size => size.Id)
                .ToListAsync();

            var invalidSizeIds = sizeIds.Distinct().Except(validSizeIds).ToList();

            if (invalidSizeIds.Count > 0)
                errors.Add(Error.Validation(nameof(ProductDto.Sizes),
                    $"Sizes do not exist or do not belong to the product's category: {string.Join(", ", invalidSizeIds)}."));
        }

        return errors;
    }

    static List<ProductSize> ToProductSizes(List<ProductSizeDto> sizes)
        => sizes
            .Select(size => new ProductSize { SizeId = size.SizeId, Stock = size.Stock })
            .ToList();
}
