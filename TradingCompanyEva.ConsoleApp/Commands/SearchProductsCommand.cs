using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Interfaces;
using TradingCompanyEva.DTO.Product;

namespace TradingCompanyEva.ConsoleApp.Commands
{
    public class SearchProductsCommand : IConsoleCommand
    {
        public string Name => "4. Пошук товару за назвою";

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public SearchProductsCommand(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public object? Execute(object? state)
        {
            
            Console.Write("Введiть назву товару для пошуку: ");
            string query = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(query))
            {
                Console.WriteLine("Запит не може бути порожнiм.");
                return null;
            }

            
            var products = _productRepository.SearchByName(query);

            if (!products.Any())
            {
                Console.WriteLine($"Товарiв за запитом '{query}' не знайдено.");
                return null;
            }

           
            var productDtos = _mapper.Map<IEnumerable<ProductDisplayDto>>(products);

            
            Console.WriteLine($"Результати пошуку ({query}):");
            foreach (var product in productDtos)
            {
                Console.WriteLine($"  [{product.ProductId}] {product.ProductName} - {product.Price:C}");
            }

            return null;
        }
    }
}
