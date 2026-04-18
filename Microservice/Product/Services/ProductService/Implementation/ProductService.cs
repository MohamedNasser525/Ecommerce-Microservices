using ProductService.Dto;
using ProductService.Dto.Request;
using ProductService.Dto.Update;
using ProductService.Models;
using ProductService.Repository.ProductRepo;

namespace ProductService.Services.ProductService.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;

        public ProductService(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public async Task<ProductDto> AddProduct(ProductRequestDto productDto)
        {
            var product = new Models.Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Quantity = productDto.Quantity,
                CategoryId = productDto.CategoryId
            };

            var result = await _productRepo.AddProduct(product);
            return MapToDto(result);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProducts()
        {
            var products = await _productRepo.GetAllProducts();
            return products.Select(MapToDto);
        }

        public async Task<ProductDto> GetProductById(Guid productId)
        {
            var product = await _productRepo.GetProductById(productId);
            return MapToDto(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryId(Guid categoryId)
        {
            var products = await _productRepo.GetAllProductsByCategoryId(categoryId);
            return products.Select(MapToDto);
        }

        public async Task<ProductDto> UpdateProduct(ProductUpdateDto productDto)
        {
            var updatedProduct = await _productRepo.UpdateProduct(productDto);
            return MapToDto(updatedProduct);
        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            return await _productRepo.DeleteProduct(productId);
        }

        private static ProductDto MapToDto(Models.Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description ?? string.Empty,
                Price = product.Price,
                Quantity = product.Quantity,
                CreatedAt = product.CreatedAt,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? string.Empty
            };
        }
    }
}
