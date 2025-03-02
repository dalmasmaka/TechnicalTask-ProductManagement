using AutoMapper;
using Microsoft.Extensions.Logging;
using PM_Application.DTOs.Category;
using PM_Application.DTOs.Product;
using PM_Application.Interfaces;
using PM_Domain.Entities;
using PM_Infrastructure.Interfaces;
using PM_Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM_Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateCategoryDTO> CreateAsync(CreateCategoryDTO categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);
                category.CreatedBy = "SomeUser";
                category.UpdatedAt = null;
                category.UpdatedBy = null;
                await _categoryRepository.CreateAsync(category);
                return _mapper.Map<CreateCategoryDTO>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a category.");
                throw new ApplicationException("An error occurred while creating the category. Please try again later.");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category != null)
                {
                    await _categoryRepository.DeleteAsync(category.Id);
                }
                
                else
                {
                    _logger.LogWarning("Category with ID " + id + " not found for deletion .");
                    throw new ApplicationException($"Category with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting category with ID" + id + ".");
                throw new ApplicationException("An error occurred while deleting the category. Please try again later.");
            }
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<CategoryDto>>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all categories.");
                throw new ApplicationException("An error occurred while fetching the categories. Please try again later.");
            }
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(id);
                if (category == null)
                {
                    _logger.LogWarning("Category with ID not found" + id + ".");
                    throw new ApplicationException($"Category with ID {id} not found.");
                }
                return _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching product with ID " + id + ".");
                throw new ApplicationException("An error occurred while fetching the product details. Please try again later.");
            }
        }

        public async Task<bool> HasProductsAsync(int categoryId)
        {
            try
            {
                if(categoryId == 0)
                {
                    _logger.LogWarning("The category Id is null");
                    throw new ApplicationException("The category Id is null");
                }
                return await _categoryRepository.HasProductsAsync(categoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching category information with ID " + categoryId + ".");
                throw new ApplicationException("An error occurred while fetching the category information details. Please try again later.");
            }
        }

        public async Task<UpdateCategoryDTO> UpdateAsync(UpdateCategoryDTO categoryDto)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(categoryDto.Id);

                if (category == null)
                {
                    throw new ApplicationException("Product not found.");
                }

                _mapper.Map(categoryDto, category);
                category.UpdatedAt = DateTime.UtcNow;
                category.UpdatedBy = "SomeUser";
                await _categoryRepository.UpdateAsync(category);
                return _mapper.Map<UpdateCategoryDTO>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with ID" + categoryDto.Id + ".");
                throw new ApplicationException("An error occurred while updating the product. Please try again later.");
            }
        }
    }
}
