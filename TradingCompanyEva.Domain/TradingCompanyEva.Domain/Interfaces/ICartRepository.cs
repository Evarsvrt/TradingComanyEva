using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Entites;

namespace TradingCompanyEva.Domain.Interfaces
{
    public interface ICartRepository : IGenericRepository<Cart>
    {
        Cart GetByUserId(int userId);

        void AddItemToCart(int cartId, int productId, int quantity);
    }
}
