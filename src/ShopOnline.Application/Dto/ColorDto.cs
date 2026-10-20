namespace ShopOnline.Application.Dto;

public record ColorDto(int Id, string Name, string HexCode);

public record CreateColorDto(string Name, string HexCode);

public record UpdateColorDto(string Name, string HexCode);
