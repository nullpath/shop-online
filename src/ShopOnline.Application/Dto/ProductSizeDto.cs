using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Application.Dto;

public record ProductSizeDto(
    [Range(1, int.MaxValue)] int SizeId,
    [Range(0, int.MaxValue)] int Stock);
