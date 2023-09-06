using AutoMapper;
using Domain.Entities;
using Services.Contracts;

namespace ConsoleApp.Mapping
{
    public class ShopMappingProfile : Profile
    {
        public ShopMappingProfile()
        {
            CreateMap<Shop, ShopDto>().ReverseMap();
        }
    }
}
