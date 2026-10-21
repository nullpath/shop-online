using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Application.Dto;

public record SizeDto(int Id, int CategoryId, string Name);

public record CreateSizeDto(
    [Range(1, int.MaxValue)] int CategoryId,
    [Required][StringLength(10)] string Name);

public record UpdateSizeDto(
    [Range(1, int.MaxValue)] int CategoryId,
    [Required][StringLength(10)] string Name);
