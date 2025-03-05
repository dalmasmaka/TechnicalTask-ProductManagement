using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PM_Application.DTOs.Category;
using PM_Application.Interfaces;

namespace PM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetById(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<CreateCategoryDTO>> Create(CreateCategoryDTO categoryDto)
        {
            var createdCategory = await _categoryService.CreateAsync(categoryDto);
            return Ok(new { Message = "Category created successfully.", Category = createdCategory });
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateCategoryDTO>> Update(int id, UpdateCategoryDTO categoryDto)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest("Category ID mismatch.");
            }
            var updatedCategory = await _categoryService.UpdateAsync(categoryDto);
            return Ok(new { Message = "Category updated successfully.", Category = updatedCategory });
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return Ok(new { Message = "Category deleted successfully." });
        }
        [HttpGet("has-products/{categoryId}")]
        public async Task<IActionResult> HasProducts(int categoryId)
        {
            bool hasProducts = await _categoryService.HasProductsAsync(categoryId);

            if (hasProducts)
            {
                return Ok(new { message = "This category has associated products. Do you want to proceed with deletion?" });
            }
            
            await _categoryService.DeleteAsync(categoryId);
            return Ok(new { message = "Category deleted successfully." });
        }
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetTotalCategoriesCount()
        {
            var totalCategoriesCount = await _categoryService.GetTotalCategoriesCountAsync();
            return Ok(totalCategoriesCount);
        }
    }
}
