using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Entites;
using TradingCompanyEva.DTO.Cart;

namespace TradingCompanyEva.ConsoleApp.AutoMapper
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            
            CreateMap<CartItem, CartItemDisplayDto>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PricePerItem, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.TotalPriceForItem, opt => opt.MapFrom(src => src.Product.Price * src.Quantity));

           
            CreateMap<Cart, CartDisplayDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems))
                
                .ForMember(dest => dest.TotalPrice,
                           opt => opt.MapFrom(src => src.CartItems.Sum(item => item.Product.Price * item.Quantity)));
        }
    }
}
