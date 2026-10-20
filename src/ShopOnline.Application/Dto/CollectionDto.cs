namespace ShopOnline.Application.Dto;

public record CollectionDto(int Id, string Name);

public record CreateCollectionDto(string Name);

public record UpdateCollectionDto(string Name);
