using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Entites;

namespace TradingCompanyEva.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetByUsername(string username);
    }

    
}
