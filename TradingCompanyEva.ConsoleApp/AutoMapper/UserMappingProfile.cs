using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Entites;
using TradingCompanyEva.DTO.User;

using AutoMapper;


namespace TradingCompanyEva.ConsoleApp.AutoMapper
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            
            CreateMap<User, LoginResponseDto>()
                .ForMember(dest => dest.Role,
                           opt => opt.MapFrom(src =>
                               src.UserRoles.Select(ur => ur.Role.RoleName).FirstOrDefault()
                           ));
        }
    }
}
