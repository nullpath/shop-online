using Microsoft.EntityFrameworkCore;
using ShopOnline.Domain.Entities;

namespace ShopOnline.Infrastructure.Data;

public class ShopDatabaseContext(DbContextOptions<ShopDatabaseContext> options) 
    : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductSize> ProductSizes => Set<ProductSize>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Subcategory> Subcategories => Set<Subcategory>();
    public DbSet<Size> Sizes => Set<Size>();
    public DbSet<Color> Colors => Set<Color>();
    public DbSet<Collection> Collections => Set<Collection>();
    public DbSet<Gender> Genders => Set<Gender>();
}