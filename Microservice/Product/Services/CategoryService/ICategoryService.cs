using ProductService.Dto;
using ProductService.Dto.Request;
using ProductService.Dto.Update;

namespace ProductService.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<CategoryDto> AddCategory(CategoryRequestDto categoryDto);
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(Guid categoryId);
        Task<CategoryDto> UpdateCategory(CategoryUpdateDto categoryDto);
        Task<bool> DeleteCategory(Guid categoryId);
    }
}
