using System.ComponentModel.DataAnnotations;

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
    [Required][StringLength(100)] string Name,
    [Range(typeof(decimal), "0.01", "100000")] decimal Price,
    [Range(1, int.MaxValue)] int SubcategoryId,
    [Range(1, int.MaxValue)] int ColorId,
    [Range(1, int.MaxValue)] int? CollectionId,
    [Range(1, int.MaxValue)] int GenderId,
    [Required] List<ProductSizeDto> Sizes);

public record UpdateProductDto(
    [Required][StringLength(100)] string Name,
    [Range(typeof(decimal), "0.01", "100000")] decimal Price,
    [Range(1, int.MaxValue)] int SubcategoryId,
    [Range(1, int.MaxValue)] int ColorId,
    [Range(1, int.MaxValue)] int? CollectionId,
    [Range(1, int.MaxValue)] int GenderId,
    [Required] List<ProductSizeDto> Sizes);
