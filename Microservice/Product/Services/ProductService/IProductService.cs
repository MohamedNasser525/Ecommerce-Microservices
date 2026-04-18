using ProductService.Dto;
using ProductService.Dto.Request;
using ProductService.Dto.Update;

namespace ProductService.Services.ProductService
{
    public interface IProductService
    {
        Task<ProductDto> AddProduct(ProductRequestDto productDto);
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto> GetProductById(Guid productId);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryId(Guid categoryId);
        Task<ProductDto> UpdateProduct(ProductUpdateDto productDto);
        Task<bool> DeleteProduct(Guid productId);
    }
}
