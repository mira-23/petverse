using AutoMapper;
using PetVerse.DTOs;
using PetVerse.Entities;

namespace PetVerse
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDTO,User>()
                .ForMember(u=>u.UserName, options => options.MapFrom(x=>x.Email));
        }
    }
}