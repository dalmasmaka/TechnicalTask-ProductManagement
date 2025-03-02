using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PM_Domain.Entities;
using PM_Infrastructure.Data;
using PM_Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    }
}
