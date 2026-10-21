using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Application.Dto;

public record GenderDto(int Id, string Name);

public record CreateGenderDto(
    [Required][StringLength(20)] string Name);

public record UpdateGenderDto(
    [Required][StringLength(20)] string Name);
