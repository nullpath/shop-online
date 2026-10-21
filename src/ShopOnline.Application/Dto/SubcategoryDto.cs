using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Application.Dto;

public record SubcategoryDto(int Id, int CategoryId, string Name);

public record CreateSubcategoryDto(
    [Range(1, int.MaxValue)] int CategoryId,
    [Required][StringLength(50)] string Name);

public record UpdateSubcategoryDto(
    [Range(1, int.MaxValue)] int CategoryId,
    [Required][StringLength(50)] string Name);
