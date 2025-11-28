using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingCompanyEva.ConsoleApp.Commands;
using TradingCompanyEva.DTO.User;


namespace TradingCompanyEva.ConsoleApp
{
    public class Menu
    {
        private readonly Dictionary<string, IConsoleCommand> _commands;
        private LoginResponseDto? _currentUser = null; 

        public Menu(IEnumerable<IConsoleCommand> commands)
        {
            
            _commands = commands.ToDictionary(
                c => c.Name.Split('.')[0].Trim(), 
                c => c                             
            );
        }

        public void Run()
        {
            
            while (_currentUser == null)
            {
                Console.Clear();
                Console.WriteLine("= ВIТАЄМО В TRADING COMPANY =");
                Console.WriteLine("Будь ласка, увiйдiть в систему.");

                

                var loginCommand = _commands.Values.FirstOrDefault(c => c.Name.Contains("Логiн"));
                if (loginCommand == null)
                {
                    Console.WriteLine("Критична помилка: Команда логiну не знайдена.");
                    return;
                }

                object? result = loginCommand.Execute(null);

                if (result is LoginResponseDto loginResponse)
                {
                    SetCurrentUser(loginResponse);
                }

                

                
            }

            
            bool running = true;
            while (running)
            {
                DisplayMenu();
                Console.Write($"\nВи увiйшли як: {_currentUser.Username}. Ваш вибiр: ");
                string input = Console.ReadLine()?.Trim() ?? "0";

                if (input == "0")
                {
                    running = false;
                    Console.WriteLine("Дякуємо! Вихiд...");
                }
                else if (_commands.TryGetValue(input, out var command))
                {
                    Console.Clear();
                    Console.WriteLine($"- Виконання: {command.Name} -");
                    command.Execute(_currentUser); 
                    Console.WriteLine("\n");
                    Console.WriteLine("Натиснiть Enter, щоб продовжити...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Невiдома команда.");
                }
            }
        }


        private void DisplayMenu()
        {
            Console.Clear();
            Console.WriteLine($"= ГОЛОВНЕ МЕНЮ =");            
            foreach (var command in _commands.Values.OrderBy(c => c.Name))
            {
                Console.WriteLine(command.Name);
            }
            Console.WriteLine("0. Вихiд");
        }

        public void SetCurrentUser(LoginResponseDto user)
        {
            _currentUser = user;
        }
    }
}
