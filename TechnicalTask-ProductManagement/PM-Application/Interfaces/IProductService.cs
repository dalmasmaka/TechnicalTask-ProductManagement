using PM_Application.DTOs.Product;
using PM_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(int id);
        Task<CreateProductDTO> CreateAsync(CreateProductDTO product);
        Task<UpdateProductDTO> UpdateAsync(UpdateProductDTO product);
        Task DeleteAsync(int id);
    }
}
