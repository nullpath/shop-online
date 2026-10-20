namespace ShopOnline.Application.Dto;

public record SizeDto(int Id, int CategoryId, string Name);

public record CreateSizeDto(int CategoryId, string Name);

public record UpdateSizeDto(int CategoryId, string Name);
