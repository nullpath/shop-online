using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Application.Dto;

public record CategoryDto(int Id, string Name);

public record CreateCategoryDto(
    [Required][StringLength(50)] string Name);

public record UpdateCategoryDto(
    [Required][StringLength(50)] string Name);
