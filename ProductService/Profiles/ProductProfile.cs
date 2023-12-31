using AutoMapper;
using ProductService.Models.Dtos;
using ProductService.Models;

namespace ProductService.Profiles
{
    public class ProductProfile : Profile
    {

        public ProductProfile()
        {
            CreateMap<AddProductDto, Product>().ReverseMap();
            CreateMap<AddProductImageDto, ProductImage>().ReverseMap();
        }
    }
}
