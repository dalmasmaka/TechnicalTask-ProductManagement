﻿using PM_Application.DTOs.Product;

namespace PM_Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(int id);
        Task<CreateProductDTO> CreateAsync(CreateProductDTO product);
        Task<UpdateProductDTO> UpdateAsync(UpdateProductDTO product);
        Task DeleteAsync(int id);
        Task<PagedResult<ProductDTO>> GetPagedProductsAsync(int pageNumber, int pageSize, string sortColumn = "Name", string sortDirection = "desc", FilterProductDTO filterObject = null);
        Task<int> GetTotalProductCountAsync();

    }
}
