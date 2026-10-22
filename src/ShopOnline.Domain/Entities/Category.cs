namespace ShopOnline.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Subcategory> Subcategories { get; set; } = [];
    public List<Size> Sizes { get; set; } = [];
}
