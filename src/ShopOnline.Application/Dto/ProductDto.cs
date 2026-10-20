namespace ShopOnline.Application.Dto;

public record ProductDto(
    int Id,
    string Name,
    decimal Price,
    int SubcategoryId,
    int ColorId,
    int? CollectionId,
    int GenderId,
    List<ProductSizeDto> Sizes);

public record CreateProductDto(
    string Name,
    decimal Price,
    int SubcategoryId,
    int ColorId,
    int? CollectionId,
    int GenderId,
    List<ProductSizeDto> Sizes);

public record UpdateProductDto(
    string Name,
    decimal Price,
    int SubcategoryId,
    int ColorId,
    int? CollectionId,
    int GenderId,
    List<ProductSizeDto> Sizes);
