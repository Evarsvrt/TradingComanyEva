using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingCompanyEva.DTO.Cart
{
    public class CartDisplayDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItemDisplayDto> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
