using ProductService.Dto.Update;
using ProductService.Models;

namespace ProductService.Repository.ProductRepo
{
    public interface IProductRepository
    {
        Task<Models.Product> AddProduct(Models.Product product);
        Task<IEnumerable<Models.Product>> GetAllProducts();
        Task<Models.Product> GetProductById(Guid productId);
        Task<IEnumerable<Models.Product>> GetAllProductsByCategoryId(Guid categoryId);
        Task<Models.Product> UpdateProduct(ProductUpdateDto product);
        Task<bool> DeleteProduct(Guid productId);
    }
}
