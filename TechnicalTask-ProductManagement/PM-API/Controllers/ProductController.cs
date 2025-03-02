using Microsoft.AspNetCore.Mvc;
using PM_Application.DTOs.Product;
using PM_Application.Interfaces;

namespace PM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<CreateProductDTO>> Create( CreateProductDTO productDto)
        {
            var createdProduct = await _productService.CreateAsync(productDto);
            return Ok(new { Message = "Product created successfully.", Product = createdProduct });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateProductDTO>> Update(int id, UpdateProductDTO productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest("Product ID mismatch.");
            }

            var updatedProduct = await _productService.UpdateAsync(productDto);
            return Ok(new { Message = "Product updated successfully.", Product = updatedProduct });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok(new { Message = "Product deleted successfully." });
        }
    }
}
