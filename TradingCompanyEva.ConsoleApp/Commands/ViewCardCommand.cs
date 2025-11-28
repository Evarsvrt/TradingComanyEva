using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Interfaces;
using TradingCompanyEva.DTO.Cart;
using TradingCompanyEva.DTO.User;

namespace TradingCompanyEva.ConsoleApp.Commands
{
    public class ViewCardCommand : IConsoleCommand
    {
        public string Name => "6. Переглянути мiй кошик";

        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public ViewCardCommand(ICartRepository cartRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public object? Execute(object? state)
        {
            if (state is not LoginResponseDto currentUser)
            {
                Console.WriteLine("Помилка: Ви маєте бути залогiненi.");
                return null;
            }

            var cart = _cartRepository.GetByUserId(currentUser.UserId);
            if (cart == null)
            {
                Console.WriteLine("Помилка: Кошик не знайдено.");
                return null;
            }

            var cartDto = _mapper.Map<CartDisplayDto>(cart);

            Console.WriteLine($"--- ВАШ КОШИК (ID: {cartDto.CartId}) ---");
            if (cartDto.Items.Count == 0)
            {
                Console.WriteLine("Кошик порожнiй.");
            }
            else
            {
                foreach (var item in cartDto.Items)
                {
                    Console.WriteLine($"  - {item.ProductName} (ID: {item.ProductId})");
                    Console.WriteLine($"    К-сть: {item.Quantity} x {item.PricePerItem:C} = {item.TotalPriceForItem:C}");
                }
            }

            Console.WriteLine("----------------------------------");
            Console.WriteLine($"  ЗАГАЛЬНА СУМА: {cartDto.TotalPrice:C}");
            return null;
        }
    }
}
