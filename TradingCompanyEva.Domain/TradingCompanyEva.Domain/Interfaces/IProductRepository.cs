using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Entites;

namespace TradingCompanyEva.Domain.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        IEnumerable<Product> GetByCategoryId(int categoryId);

        
        IEnumerable<Product> GetByCategoryIdSorted(int categoryId, string sortBy, bool ascending);

        IEnumerable<Product> SearchByName(string nameFragment);
    }
}
