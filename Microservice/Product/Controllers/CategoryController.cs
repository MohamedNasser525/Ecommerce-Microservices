using Microsoft.AspNetCore.Mvc;
using ProductService.Dto.Request;
using ProductService.Dto.Update;
using ProductService.Services.CategoryService;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("it work Category Service");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllCategories();
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _categoryService.GetCategoryById(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryRequestDto category)
        {
            var result = await _categoryService.AddCategory(category);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            if (categoryUpdateDto.Id != Guid.Empty && categoryUpdateDto.Id != id)
            {
                return BadRequest(new { message = "Category id in body must match route id." });
            }

            categoryUpdateDto.Id = id;

            try
            {
                var result = await _categoryService.UpdateCategory(categoryUpdateDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _categoryService.DeleteCategory(id);
            if (!deleted)
            {
                return NotFound(new { message = "Category not found or already deleted." });
            }

            return Ok("delete succeeded");
        }
    }
}
