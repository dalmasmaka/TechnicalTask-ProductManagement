﻿using PM_Application.DTOs.Category;
using PM_Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<CreateCategoryDTO> CreateAsync(CreateCategoryDTO categoryDto);
        Task<UpdateCategoryDTO> UpdateAsync(UpdateCategoryDTO categoryDto);
        Task DeleteAsync(int id);
        Task<bool> HasProductsAsync(int categoryId);
        Task<int> GetTotalCategoriesCountAsync();
    }
}
