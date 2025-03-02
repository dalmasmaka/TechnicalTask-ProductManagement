using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PM_Domain.Entities;
using PM_Infrastructure.Data;
using PM_Infrastructure.Interfaces;


namespace PM_Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task CreateAsync(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a category.");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                //duhet me u implementu ne service ky kusht 
                bool hasProducts = await HasProductsAsync(id);
                if (hasProducts)
                {
                    var products = await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
                    foreach (var product in products)
                    {
                        product.isDeleted = true;
                    }
                }

                category.isDeleted = true;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting a category.");
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                return await _context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products.");
                throw;
            }
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            try
            {
                 return await _context.Categories.FindAsync(id);
                
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching category with ID " + id + ".");
                throw;
            }
        }

        public async Task<bool> HasProductsAsync(int categoryId)
        {
            try
            {
                return await _context.Products.AnyAsync(p => p.CategoryId == categoryId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task UpdateAsync(Category category)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with ID  " + category.Id + ".");
                throw;
            }
        }
    }
}
