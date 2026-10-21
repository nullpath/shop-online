using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Application.Dto;

public record ColorDto(int Id, string Name, string HexCode);

public record CreateColorDto(
    [Required][StringLength(30)] string Name,
    [Required][RegularExpression("^#[0-9A-Fa-f]{6}$")] string HexCode);

public record UpdateColorDto(
    [Required][StringLength(30)] string Name,
    [Required][RegularExpression("^#[0-9A-Fa-f]{6}$")] string HexCode);
