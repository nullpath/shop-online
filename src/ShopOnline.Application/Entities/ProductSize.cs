namespace ShopOnline.Application.Entities;

public class ProductSize
{
    public int Id { get; set; }
    public int Stock { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int SizeId { get; set; }
    public Size Size { get; set; } = null!;
}
