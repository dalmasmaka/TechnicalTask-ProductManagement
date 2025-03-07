using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PM_Domain.Entities;
using PM_Infrastructure.Data;
using PM_Infrastructure.DTOs;
using PM_Infrastructure.Interfaces;

namespace PM_Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products.");
                throw;  
            }
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            try
            {
               return await _context.Products.FindAsync(id);
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product with ID "+id+".");
                throw;
            }
        }

        public async Task CreateAsync(Product product)
        {
            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a product.");
                throw;
            }
        }

        public async Task UpdateAsync(Product product)
        {
            try
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {   
                _logger.LogError(ex, "Error occurred while updating product with ID  "+ product.Id + ".");
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {

                var product = await _context.Products.FindAsync(id);
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
               
               
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with ID " +id+ ".");
                throw;
            }
        }

        public async Task<PagedResult<Product>> GetProductSearchAdvanceAsync(string name, string status, int? categoryId, string sortColumn, string sortDirection, int pageSize, int page)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(p => p.Status == status);
            }

            if (categoryId != 0 && categoryId != null)
            {
                query = query.Where(p => p.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = sortDirection == "desc"
                    ? query.OrderByDescending(e => EF.Property<Product>(e, sortColumn))
                    : query.OrderBy(e => EF.Property<Product>(e, sortColumn));
            }

            var products = await query.Include(x => x.Category)
            .Skip((page - 1) * pageSize) // Skip products for previous pages
            .Take(pageSize) // Take the number of products based on pageSize
            .ToListAsync();

            var totalCount = await query.CountAsync();

            return new PagedResult<Product>
            {
                Items = products,
                TotalCount = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };
        }
        public async Task<int> CountAsync()
        {
            return await _context.Products.CountAsync();
        }

    }
}
