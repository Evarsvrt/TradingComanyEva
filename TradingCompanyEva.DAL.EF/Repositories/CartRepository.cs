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
    public class CartRepository : GenericRepository<Cart>,ICartRepository
    {
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }

        
        public Cart GetByUserId(int userId)
        {
            
            return _dbSet
                .Include(c => c.CartItems)       
                .ThenInclude(ci => ci.Product) 
                .FirstOrDefault(c => c.UserId == userId);
        }

        
        public void AddItemToCart(int cartId, int productId, int quantity)
        {
            var existingItem = _context.CartItems
                .FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId);

            if (existingItem != null)
            {

                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.CartItems.Add(newItem);
            }

            _context.SaveChanges();
        }
    }
}
