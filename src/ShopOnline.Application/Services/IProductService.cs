using ErrorOr;
using ShopOnline.Application.Dto;

namespace ShopOnline.Application.Services;

public interface IProductService
{
    Task<List<ProductDto>> GetProductsAsync(ProductFilter filter);

    Task<ErrorOr<ProductDto>> GetProductAsync(int id);

    Task<ErrorOr<ProductDto>> CreateProductAsync(CreateProductDto dto);

    Task<ErrorOr<Updated>> UpdateProductAsync(int id, UpdateProductDto dto);

    Task<ErrorOr<Deleted>> DeleteProductAsync(int id);
}
