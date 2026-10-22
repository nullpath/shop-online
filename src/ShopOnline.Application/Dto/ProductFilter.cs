using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Application.Dto;

public record ProductFilter(
    [Range(1, int.MaxValue)] int? CategoryId,
    [Range(1, int.MaxValue)] int? SubcategoryId,
    [Range(1, int.MaxValue)] int? SizeId,
    [Range(1, int.MaxValue)] int? ColorId,
    [Range(1, int.MaxValue)] int? CollectionId,
    [Range(1, int.MaxValue)] int? GenderId,
    [Range(typeof(decimal), "0", "100000")] decimal? MinPrice,
    [Range(typeof(decimal), "0", "100000")] decimal? MaxPrice,
    ProductSort? SortBy);
