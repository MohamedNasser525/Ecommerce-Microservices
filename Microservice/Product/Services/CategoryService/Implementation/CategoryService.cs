using ProductService.Dto;
using ProductService.Dto.Request;
using ProductService.Dto.Update;
using ProductService.Models;
using ProductService.Repository.CategoryRepo;

namespace ProductService.Services.CategoryService.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepo = categoryRepository;
        }

        public async Task<CategoryDto> AddCategory(CategoryRequestDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name
            };

            await _categoryRepo.AddCategory(category);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                CreatedAt = category.CreatedAt
            };
        }

        public async Task<bool> DeleteCategory(Guid categoryId)
        {
            return await _categoryRepo.DeleteCategory(categoryId);
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            return await _categoryRepo.GetAllCategories();
        }

        public async Task<CategoryDto> GetCategoryById(Guid categoryId)
        {
            var category = await _categoryRepo.GetCategoryById(categoryId);

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                CreatedAt = category.CreatedAt
            };
        }

        public async Task<CategoryDto> UpdateCategory(CategoryUpdateDto categoryDto)
        {
            var updatedCategory = await _categoryRepo.UpdateCategory(categoryDto);

            return new CategoryDto
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name,
                CreatedAt = updatedCategory.CreatedAt
            };
        }
    }
}
