using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Entites;
using TradingCompanyEva.DTO.Product;

namespace TradingCompanyEva.ConsoleApp.AutoMapper
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
           
            CreateMap<Product, ProductDisplayDto>();
        }
    }
}
