using Microsoft.EntityFrameworkCore;
using ProductService.Dto;
using ProductService.Dto.Update;
using ProductService.Models;

namespace ProductService.Repository.CategoryRepo.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category> AddCategory(Category category)
        {
            await _dbContext.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategory(Guid categoryId)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == categoryId && c.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return false;
            }

            category.DeletedAt = DateTime.UtcNow;
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var categories = await _dbContext.Categories
                .Where(c => c.DeletedAt == null)
                .AsNoTracking()
                .ToListAsync();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                CreatedAt = c.CreatedAt
            });
        }

        public async Task<Category> GetCategoryById(Guid categoryId)
        {
            var category = await _dbContext.Categories
                .Where(c => c.Id == categoryId && c.DeletedAt == null)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new Exception("The category Not Found");
            }

            return category;
        }

        public async Task<Category> UpdateCategory(CategoryUpdateDto category)
        {
            var existedCategory = await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Id == category.Id && c.DeletedAt == null);

            if (existedCategory == null)
            {
                throw new Exception("The category Not Found");
            }

            if (!string.IsNullOrWhiteSpace(category.NewName))
                existedCategory.Name = category.NewName;

            existedCategory.UpdatedAt = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
            return existedCategory;
        }
    }
}
