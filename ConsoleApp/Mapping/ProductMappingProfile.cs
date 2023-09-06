using AutoMapper;
using Domain.Entities;
using Services.Contracts;

namespace ConsoleApp.Mapping
{
    public class ProductMappingProfile :Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}
