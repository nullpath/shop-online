namespace ShopOnline.Application.Dto;

public record GenderDto(int Id, string Name);

public record CreateGenderDto(string Name);

public record UpdateGenderDto(string Name);
