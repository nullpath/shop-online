using ShopOnline.Application.Dto;
using ShopOnline.Domain.Entities;

namespace ShopOnline.Application.Mapping;

public static class DtoMappingExtensions
{
    public static CategoryDto ToDto(this Category category)
        => new(category.Id, category.Name);

    public static SubcategoryDto ToDto(this Subcategory subcategory)
        => new(subcategory.Id, subcategory.CategoryId, subcategory.Name);

    public static SizeDto ToDto(this Size size)
        => new(size.Id, size.CategoryId, size.Name);

    public static ColorDto ToDto(this Color color)
        => new(color.Id, color.Name, color.HexCode);

    public static CollectionDto ToDto(this Collection collection)
        => new(collection.Id, collection.Name);

    public static GenderDto ToDto(this Gender gender)
        => new(gender.Id, gender.Name);

    public static ProductSizeDto ToDto(this ProductSize productSize)
        => new(productSize.SizeId, productSize.Stock);

    public static ProductDto ToDto(this Product product)
        => new(
            product.Id,
            product.Name,
            product.Price,
            product.SubcategoryId,
            product.ColorId,
            product.CollectionId,
            product.GenderId,
            product.Sizes.Select(size => size.ToDto()).ToList());
}
