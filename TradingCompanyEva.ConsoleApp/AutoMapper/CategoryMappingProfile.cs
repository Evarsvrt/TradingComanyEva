using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Entites;
using TradingCompanyEva.DTO.Category;

namespace TradingCompanyEva.ConsoleApp.AutoMapper
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            
            CreateMap<Category, CategoryDisplayDto>();
        }
    }
}
