using Microsoft.Extensions.Logging;
using PM_Infrastructure.Interfaces;
using PM_Application.DTOs.Product;
using PM_Application.Interfaces;
using AutoMapper;
using PM_Domain.Entities;

namespace PM_Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper; 
        private readonly ILogger<ProductService> _logger;

        public ProductService(IProductRepository productRepository, IMapper mapper, ILogger<ProductService> logger, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper; 
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<CreateProductDTO> CreateAsync(CreateProductDTO productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto); 
                product.CreatedBy = "SomeUser";
                product.UpdatedAt = null;
                product.UpdatedBy = null;
                product.isDeleted = false;
                await _productRepository.CreateAsync(product);
                return _mapper.Map<CreateProductDTO>(product); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a product.");
                throw new ApplicationException("An error occurred while creating the product. Please try again later.");
            }
        }

        public async Task<ProductDTO> GetByIdAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Product with ID not found" + id + ".");
                    throw new KeyNotFoundException($"Product with ID {id} not found.");

                }
                return _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new KeyNotFoundException("An error occurred while fetching the product details. Please try again later.");
            }
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                if (!products.Any())
                {
                    _logger.LogInformation("No products found.");
                    throw new KeyNotFoundException("No products found.");

                }
                return _mapper.Map<IEnumerable<ProductDTO>>(products); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all products.");
                throw new KeyNotFoundException("An error occurred while fetching the products. Please try again later.");
            }
        }

        public async Task<UpdateProductDTO> UpdateAsync(UpdateProductDTO productDto)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productDto.Id);

                if (product == null)
                {
                    throw new KeyNotFoundException("Product not found.");
                }

                _mapper.Map(productDto, product);
                product.UpdatedAt = DateTime.UtcNow;
                product.UpdatedBy = "SomeUser"; 
                await _productRepository.UpdateAsync(product);
                return _mapper.Map<UpdateProductDTO>(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with ID"+ productDto.Id + ".");
                throw new ApplicationException("An error occurred while updating the product. Please try again later.");
            }
        }


        public async Task DeleteAsync(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product != null)
                {
                        product.isDeleted = true;
                        await _productRepository.DeleteAsync(product.Id);
                }
                else
                {
                    _logger.LogWarning("Product with ID " + id + " not found for deletion .");
                    throw new KeyNotFoundException($"Product with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with ID" + id + ".");
                throw new KeyNotFoundException("An error occurred while deleting the product. Please try again later.");
            }
        }
        public async Task<PagedResult<ProductDTO>> GetPagedProductsAsync(int pageNumber, int pageSize, string sortColumn = "Name", string sortDirection = "desc", FilterProductDTO filterObject = null)
        {
            try
            {
                var res = await _productRepository.GetProductSearchAdvanceAsync(filterObject.Name, filterObject.Status, filterObject.CategoryId, sortColumn, sortDirection, pageSize, pageNumber);
                return new PagedResult<ProductDTO>
                {
                    Items = _mapper.Map<IEnumerable<ProductDTO>>(res.Items),
                    TotalCount = res.TotalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "An error occurred while retrieving paged products.");
                throw new ApplicationException("An error occurred while retrieving paged products.", ex);
            }
        }
        public async Task<int> GetTotalProductCountAsync()
        {
            try
            {
                return await _productRepository.CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "An error occurred while retrieving total product count.");
                throw new ApplicationException("An error occurred while retrieving total product count.", ex);
            }
        }

    }
}
