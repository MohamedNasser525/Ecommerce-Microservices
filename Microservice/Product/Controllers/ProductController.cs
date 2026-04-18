using Microsoft.AspNetCore.Mvc;
using ProductService.Dto.Request;
using ProductService.Dto.Update;
using ProductService.Services.ProductService;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("it work Product Service");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductById(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("category/{categoryId:guid}")]
        public async Task<IActionResult> GetByCategoryId(Guid categoryId)
        {
            var products = await _productService.GetProductsByCategoryId(categoryId);
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] ProductRequestDto productDto)
        {
            var result = await _productService.AddProduct(productDto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ProductUpdateDto updateDto)
        {
            if (updateDto.Id != Guid.Empty && updateDto.Id != id)
            {
                return BadRequest(new { message = "Product id in body must match route id." });
            }

            updateDto.Id = id;

            try
            {
                var result = await _productService.UpdateProduct(updateDto);
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
            var deleted = await _productService.DeleteProduct(id);
            if (!deleted)
            {
                return NotFound(new { message = "Product not found or already deleted." });
            }

            return Ok("delete succeeded");
        }
    }
}
