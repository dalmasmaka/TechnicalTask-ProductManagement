using AutoMapper;
using PM_Application.DTOs.Product;
using PM_Domain.Entities;

namespace PM_Application.MappingProfiles
{
    class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<ProductDTO, Product>();

            CreateMap<CreateProductDTO, Product>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))  // Set CreatedAt on creation
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // Don't set UpdatedAt on create, it's null
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Don't set CreatedBy here, it's set in Service Layer
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore()) // Don't set UpdatedBy here, it's set in Service Layer
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));


            CreateMap<UpdateProductDTO, Product>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Prevent modification
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
            .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
        }
    }
}
