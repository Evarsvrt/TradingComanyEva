using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using TradingCompanyEva.Domain.Interfaces;
using TradingCompanyEva.DTO;
using TradingCompanyEva.DTO.Product;

namespace TradingCompanyEva.ConsoleApp.Commands
{
    public class ShowProductsByCategoryCommand : IConsoleCommand
    {
        public string Name => "3. Переглянути товари в категорiї";

        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ShowProductsByCategoryCommand(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public object? Execute(object? state)
        {
            
            Console.Write("Введiть ID категорiї, яку бажаєте переглянути: ");
            if (!int.TryParse(Console.ReadLine(), out int categoryId))
            {
                Console.WriteLine("Неправильний ID.");
                return null;
            }

            Console.Write("Сортувати за назвою чи цiною?: ");
            string sortByInput = Console.ReadLine()?.ToUpper() ?? "N";
            string sortBy = (sortByInput == "P") ? "Price" : "Name";

            Console.Write("За зростанням чи спаданням? : ");
            string sortOrderInput = Console.ReadLine()?.ToUpper() ?? "A";
            bool ascending = (sortOrderInput != "D");

            var products = _productRepository.GetByCategoryIdSorted(categoryId, sortBy, ascending);

            if (!products.Any())
            {
                Console.WriteLine("В цiй категорiї немає товарiв або категорiя не iснує.");
                return null;
            }

            var productDtos = _mapper.Map<IEnumerable<ProductDisplayDto>>(products);

            Console.WriteLine($"Товари в категорiї {categoryId} (вiдсортовано):");
            foreach (var product in productDtos)
            {
                Console.WriteLine($"  [{product.ProductId}] {product.ProductName} - {product.Price:C}");
                Console.WriteLine($"      {product.Description}");
            }
            return null;
        }
    }
}
