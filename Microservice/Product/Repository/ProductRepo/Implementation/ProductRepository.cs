using Microsoft.EntityFrameworkCore;
using ProductService.Dto.Update;
using ProductService.Models;

namespace ProductService.Repository.ProductRepo.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Models.Product> AddProduct(Models.Product product)
        {
            await _dbContext.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            var product = await _dbContext.Products
                .Where(p => p.Id == productId && p.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return false;
            }

            product.DeletedAt = DateTime.UtcNow;
            _dbContext.Products.Update(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Models.Product>> GetAllProducts()
        {
            return await _dbContext.Products
                .Include(p => p.Category)
                .Where(p => p.DeletedAt == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Models.Product> GetProductById(Guid productId)
        {
            var product = await _dbContext.Products
                .Include(p => p.Category)
                .Where(p => p.Id == productId && p.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new Exception("The Product Not Found");
            }

            return product;
        }

        public async Task<IEnumerable<Models.Product>> GetAllProductsByCategoryId(Guid categoryId)
        {
            return await _dbContext.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId && p.DeletedAt == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Models.Product> UpdateProduct(ProductUpdateDto product)
        {
            var existedProduct = await _dbContext.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == product.Id && p.DeletedAt == null);

            if (existedProduct == null)
            {
                throw new Exception("The Product Not Found");
            }

            if (!string.IsNullOrWhiteSpace(product.Name))
                existedProduct.Name = product.Name;

            if (product.Description != null)
                existedProduct.Description = product.Description;

            if (product.Price.HasValue)
                existedProduct.Price = product.Price.Value;

            if (product.Quantity.HasValue)
                existedProduct.Quantity = product.Quantity.Value;

            if (product.CategoryId.HasValue && product.CategoryId.Value != Guid.Empty)
                existedProduct.CategoryId = product.CategoryId.Value;

            existedProduct.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return existedProduct;
        }
    }
}
