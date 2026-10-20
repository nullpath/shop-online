namespace ShopOnline.Application.Dto;

public record SubcategoryDto(int Id, int CategoryId, string Name);

public record CreateSubcategoryDto(int CategoryId, string Name);

public record UpdateSubcategoryDto(int CategoryId, string Name);
