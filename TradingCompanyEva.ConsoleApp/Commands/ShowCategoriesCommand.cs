using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Interfaces;
using TradingCompanyEva.DTO.User;
using AutoMapper;
using TradingCompanyEva.DTO.Category;
namespace TradingCompanyEva.ConsoleApp.Commands
{
    public class ShowCategoriesCommand : IConsoleCommand
    {
        public string Name => "2. Переглянути категорiї товарiв";

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ShowCategoriesCommand(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public object? Execute(object? state)
        {
            
            var categories = _categoryRepository.GetAll();

            
            var categoryDtos = _mapper.Map<IEnumerable<CategoryDisplayDto>>(categories);

           
            Console.WriteLine("Доступнi категорiї:");
            foreach (var category in categoryDtos)
            {
                Console.WriteLine($"  [{category.CategoryId}] - {category.CategoryName}");
            }

            return null;
        }
    }
}
