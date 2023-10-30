﻿using AutoMapper;
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
               .ForMember(dest => dest.Email, opt => opt.MapFrom(scr => scr.Email))
               .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(scr => scr.IsVerified))
               .ForMember(dest => dest.VerificationToken, opt => opt.MapFrom(scr => scr.VerificationToken));

            CreateMap<UserDto, User>()
               .ForMember(dest => dest.FirstName, opt => opt.MapFrom(scr => scr.FirstName))
               .ForMember(dest => dest.LastName, opt => opt.MapFrom(scr => scr.LastName))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(scr => scr.Email))
               .ForMember(dest => dest.VerificationToken, opt => opt.MapFrom(scr => scr.VerificationToken))
               .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(scr => scr.IsVerified))
               .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
