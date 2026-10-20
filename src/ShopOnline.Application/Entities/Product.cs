namespace ShopOnline.Application.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public int SubcategoryId { get; set; }
    public Subcategory Subcategory { get; set; } = null!;

    public int ColorId { get; set; }
    public Color Color { get; set; } = null!;

    public int? CollectionId { get; set; }
    public Collection? Collection { get; set; }

    public int GenderId { get; set; }
    public Gender Gender { get; set; } = null!;

    public List<ProductSize> Sizes { get; set; } = [];
}
