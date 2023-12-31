using AutoMapper;
using CartService.Models.Dtos;
using CartService.Models;

namespace ProductService.Profiles
{
    public class CartProfile : Profile
    {

        public CartProfile()
        {
            CreateMap<AddCartDto, Cart>().ReverseMap();
            CreateMap<AddCartItemDto, CartItem>().ReverseMap();
        }
    }
}
