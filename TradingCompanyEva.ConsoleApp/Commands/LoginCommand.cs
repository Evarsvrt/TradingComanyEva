using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.Domain.Interfaces;
using TradingCompanyEva.DTO.User;

namespace TradingCompanyEva.ConsoleApp.Commands
{
    public class LoginCommand : IConsoleCommand
    {
        public string Name => "1. Логiн"; 

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        
            
        public LoginCommand(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public object? Execute(object? state)
        {
            Console.Write("Введiть логiн: ");
            string username = Console.ReadLine();

            Console.Write("Введiть пароль: ");
            string password = Console.ReadLine(); 

            
            var loginDto = new LoginRequestDto { Username = username, Password = password };

            
            var user = _userRepository.GetByUsername(loginDto.Username);

            
            if (user != null && user.PasswordHash == (loginDto.Password + "_hashed"))
            {
                
                var loginResponse = _mapper.Map<LoginResponseDto>(user);

                
                

                Console.WriteLine($"\n Вхiд успiшний! Вiтаємо, {loginResponse.Username} (Роль: {loginResponse.Role}).");
                Console.WriteLine("Натиснiть Enter, щоб увiйти в головне меню...");
                Console.ReadLine();

                return loginResponse;
            }
            else
            {
                Console.WriteLine("\n Помилка логiну: Неправильний логiн або пароль.");
                Console.WriteLine("Натиснiть Enter, щоб спробувати ще раз...");
                Console.ReadLine();
                return null;
            }
        }
    }
}
