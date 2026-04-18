using ProductService.Dto;
using ProductService.Dto.Update;
using ProductService.Models;

namespace ProductService.Repository.CategoryRepo
{
    public interface ICategoryRepository
    {
        Task<Category> AddCategory(Category category);
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<Category> GetCategoryById(Guid categoryId);
        Task<Category> UpdateCategory(CategoryUpdateDto category);
        Task<bool> DeleteCategory(Guid categoryId);
    }
}
