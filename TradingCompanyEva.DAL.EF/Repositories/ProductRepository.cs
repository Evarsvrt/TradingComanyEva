using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Entites;
using TradingCompanyEva.Domain.Interfaces;

namespace TradingCompanyEva.DAL.EF.Repositories
{
    public class ProductRepository : GenericRepository<Product>,IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetByCategoryId(int categoryId)
        {
            return _dbSet.Where(p => p.CategoryId == categoryId).AsNoTracking().ToList();
        }

        public IEnumerable<Product> GetByCategoryIdSorted(int categoryId, string sortBy, bool ascending)
        {
            var query = _dbSet.Where(p => p.CategoryId == categoryId);

            if (sortBy == "Price")
            {
                query = ascending ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price);
            }
            else 
            {
                query = ascending ? query.OrderBy(p => p.ProductName) : query.OrderByDescending(p => p.ProductName);
            }

            return query.AsNoTracking().ToList();
        }

        public IEnumerable<Product> SearchByName(string nameFragment)
        {
            return _dbSet.Where(p => p.ProductName.Contains(nameFragment)).AsNoTracking().ToList();
        }
    }
}
