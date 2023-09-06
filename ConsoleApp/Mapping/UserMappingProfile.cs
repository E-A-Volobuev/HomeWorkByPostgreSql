using AutoMapper;
using Domain.Entities;
using Services.Contracts;

namespace ConsoleApp.Mapping
{
    public class UserMappingProfile:Profile
    {
        public UserMappingProfile() 
        {
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
