using AutoMapper;
using PM_Application.DTOs.User;
using PM_Domain.Entities;

namespace PM_Application.MappingProfiles
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<ApplicationUser, UserDTO>()
             .ForMember(dest => dest.LoginTimestamp, opt => opt.MapFrom(src => src.LoginTimestamp ?? DateTime.MinValue))
             .ForMember(dest => dest.LogOutTimestamp, opt => opt.MapFrom(src => src.LogOutTimestamp ?? DateTime.MinValue))
             .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)) 
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }



}
