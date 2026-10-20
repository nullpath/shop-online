using ShopOnline.Application.Entities;

namespace ShopOnline.Infrastructure.Data;

public class SeedData
{
    public List<Gender> Genders { get; } =
    [
        new() { Name = "Women" },
        new() { Name = "Men" },
        new() { Name = "Unisex" }
    ];

    public List<Color> Colors { get; } =
    [
        new() { Name = "Black", HexCode = "#000000" },
        new() { Name = "White", HexCode = "#FFFFFF" },
        new() { Name = "Grey", HexCode = "#808080" },
        new() { Name = "Navy", HexCode = "#1F2A44" },
        new() { Name = "Beige", HexCode = "#D9C3A0" },
        new() { Name = "Olive", HexCode = "#556B2F" }
    ];

    public List<Collection> Collections { get; } =
    [
        new() { Name = "Winter Collection" },
        new() { Name = "Summer Collection" },
        new() { Name = "Eden Smith Collection" }
    ];

    public List<Category> Categories { get; } =
    [
        new()
        {
            Name = "T-Shirts",
            Subcategories = [new() { Name = "Basic" }, new() { Name = "Longsleeve" }, new() { Name = "Polo" }],
            Sizes = [new() { Name = "XS" }, new() { Name = "S" }, new() { Name = "M" }, new() { Name = "L" }, new() { Name = "XL" }, new() { Name = "XXL" }]
        },
        new()
        {
            Name = "Gloves",
            Subcategories = [new() { Name = "Five-Finger" }, new() { Name = "Mittens" }, new() { Name = "Fingerless" }],
            Sizes = [new() { Name = "S" }, new() { Name = "M" }, new() { Name = "L" }]
        },
        new()
        {
            Name = "Pants",
            Subcategories =
            [
                new() { Name = "Chinos" }, new() { Name = "Tapered" }, new() { Name = "Joggers" }, new() { Name = "Cargo" },
                new() { Name = "Sweatpants" }, new() { Name = "Shorts" }, new() { Name = "Dress Pants" }
            ],
            Sizes = [new() { Name = "30" }, new() { Name = "32" }, new() { Name = "34" }, new() { Name = "36" }]
        }
    ];

    public List<Product> Products { get; }

    public SeedData()
    {
        Products =
        [
            NewProduct("Basic Cotton Tee", 49.99m, "T-Shirts", "Basic", "White", "Unisex", null,
                ("XS", 5), ("S", 12), ("M", 20), ("L", 15), ("XL", 8), ("XXL", 3)),
            NewProduct("Everyday Longsleeve", 79.99m, "T-Shirts", "Longsleeve", "Navy", "Women", "Winter Collection",
                ("XS", 4), ("S", 10), ("M", 7), ("L", 2)),
            NewProduct("Pique Polo", 119.99m, "T-Shirts", "Polo", "Navy", "Men", "Summer Collection",
                ("M", 6), ("L", 9), ("XL", 4)),
            NewProduct("Leather Gloves", 129.99m, "Gloves", "Five-Finger", "Black", "Men", "Eden Smith Collection",
                ("M", 5), ("L", 3)),
            NewProduct("Wool Mittens", 59.99m, "Gloves", "Mittens", "Grey", "Unisex", "Winter Collection",
                ("S", 8), ("M", 10), ("L", 6)),
            NewProduct("Knit Fingerless Gloves", 39.99m, "Gloves", "Fingerless", "Black", "Women", null,
                ("S", 7), ("M", 0)),
            NewProduct("Classic Chinos", 199.99m, "Pants", "Chinos", "Beige", "Men", "Summer Collection",
                ("30", 5), ("32", 10), ("34", 8), ("36", 3)),
            NewProduct("Slim Tapered Pants", 179.99m, "Pants", "Tapered", "Black", "Women", null,
                ("30", 6), ("32", 4)),
            NewProduct("Utility Cargo Pants", 219.99m, "Pants", "Cargo", "Olive", "Men", "Eden Smith Collection",
                ("32", 7), ("34", 5), ("36", 2)),
            NewProduct("Comfort Joggers", 149.99m, "Pants", "Joggers", "Grey", "Unisex", "Winter Collection",
                ("30", 9), ("32", 11), ("34", 6))
        ];
    }

    private Product NewProduct(string name, decimal price, string category, string subcategory, string color,
        string gender, string? collection, params (string Size, int Stock)[] sizes)
    {
        var productCategory = Categories.Single(c => c.Name == category);

        return new Product
        {
            Name = name,
            Price = price,
            Subcategory = productCategory.Subcategories.Single(s => s.Name == subcategory),
            Color = Colors.Single(c => c.Name == color),
            Gender = Genders.Single(g => g.Name == gender),
            Collection = collection is null ? null : Collections.Single(c => c.Name == collection),
            Sizes = sizes
                .Select(s => new ProductSize { Size = productCategory.Sizes.Single(x => x.Name == s.Size), Stock = s.Stock })
                .ToList()
        };
    }
}
