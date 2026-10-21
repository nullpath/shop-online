using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Application.Dto;

public record CollectionDto(int Id, string Name);

public record CreateCollectionDto(
    [Required][StringLength(50)] string Name);

public record UpdateCollectionDto(
    [Required][StringLength(50)] string Name);
