using AutoMapper;
using PM_Application.DTOs.Category;
using PM_Domain.Entities;

namespace PM_Application.MappingProfiles
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<CategoryDto, Category>();

            CreateMap<CreateCategoryDTO, Category>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<UpdateCategoryDTO, Category>()
            .ForMember(dest => dest.Products, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
            .ForMember(dest => dest.isDeleted, opt => opt.Ignore());


        }
    }
}
