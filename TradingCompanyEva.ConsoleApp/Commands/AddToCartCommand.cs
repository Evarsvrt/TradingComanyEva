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
    public class AddToCartCommand : IConsoleCommand
    {
        public string Name => "5. Додати товар до кошика";

        private readonly ICartRepository _cartRepository;

        public AddToCartCommand(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public object? Execute(object? state)
        {
            if (state is not LoginResponseDto currentUser)
            {
                Console.WriteLine("Помилка: Ви маєте бути залогiненi, щоб додавати товари.");
                return null;
            }

            var cart = _cartRepository.GetByUserId(currentUser.UserId);
            if (cart == null)
            {
                Console.WriteLine("Помилка: Кошик для цього користувача не знайдено.");
                return null;
            }

            Console.Write("Введiть ID товару, який бажаєте додати: ");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Неправильний ID товару.");
                return null;
            }

            Console.Write("Введiть кiлькiсть: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                quantity = 1;
            }

            var addToCartDto = new AddToCartDto
            {
                CartId = cart.CartId,
                ProductId = productId,
                Quantity = quantity
            };

            try
            {
                _cartRepository.AddItemToCart(addToCartDto.CartId, addToCartDto.ProductId, addToCartDto.Quantity);
                Console.WriteLine($"\n Товар (ID: {productId}) додано до кошика.");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n Помилка: Не вдалося додати товар. Переконайтеся, що ID товару правильний. ({ex.Message})");
                return null;
            }
        }
    }
}
