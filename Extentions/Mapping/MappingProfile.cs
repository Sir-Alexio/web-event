using AutoMapper;
using WebEvent.API.Model.DTO;
using WebEvent.API.Model.Entity;

namespace WebEvent.API.Extentions.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
               .ForMember(dest => dest.FirstName, opt => opt.MapFrom(scr => scr.FirstName))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(scr => scr.LastName))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(scr => scr.Email));

            CreateMap<UserDto, User>()
               .ForMember(dest => dest.FirstName, opt => opt.MapFrom(scr => scr.FirstName))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(scr => scr.LastName))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(scr => scr.Email))
               .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
