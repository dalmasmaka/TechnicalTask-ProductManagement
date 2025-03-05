using PM_Domain.Entities;
using PM_Infrastructure.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Infrastructure.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<PagedResult<Product>> GetProductSearchAdvanceAsync(string name, string status, int? categoryId, string sortColumn, string sortDirection, int pageSize, int page);
        Task<Product> GetByIdAsync(int id);
        Task CreateAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);
        Task<int> CountAsync();
    }

}
