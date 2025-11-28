
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO;
using System;
using TradingCompanyEva.Domain.Interfaces;
using TradingCompanyEva.DAL.EF;
using TradingCompanyEva.DAL.EF.Repositories;
using TradingCompanyEva.ConsoleApp.AutoMapper;
using TradingCompanyEva.ConsoleApp;
using TradingCompanyEva.ConsoleApp.Commands;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsetting.json", optional: false, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();

services.AddSingleton<IConfiguration>(configuration);
services.AddLogging(configure => configure.AddConsole());

services.AddDbContext<ApplicationDbContext>(options =>
{
    string connectionString = configuration.GetConnectionString("TradingDBConnection");
    Console.WriteLine(connectionString);
    options.UseSqlServer(connectionString);
});

services.AddAutoMapper(config =>
{
    config.AddProfile<TradingCompanyEva.ConsoleApp.AutoMapper.UserMappingProfile>();
    config.AddProfile<TradingCompanyEva.ConsoleApp.AutoMapper.ProductMappingProfile>();
    config.AddProfile<TradingCompanyEva.ConsoleApp.AutoMapper.CategoryMappingProfile>();
    config.AddProfile<TradingCompanyEva.ConsoleApp.AutoMapper.CartMappingProfile>();
});

services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<ICategoryRepository, CategoryRepository>();
services.AddScoped<IProductRepository, ProductRepository>();
services.AddScoped<ICartRepository, CartRepository>();

services.AddSingleton<Menu>();
services.AddScoped<IConsoleCommand, LoginCommand>();
services.AddScoped<IConsoleCommand, ShowCategoriesCommand>();
services.AddScoped<IConsoleCommand, ShowProductsByCategoryCommand>();
services.AddScoped<IConsoleCommand, SearchProductsCommand>();
services.AddScoped<IConsoleCommand, AddToCartCommand>();
services.AddScoped<IConsoleCommand, ViewCardCommand>();


var serviceProvider = services.BuildServiceProvider();

Console.WriteLine("Конфiгурацiя успiшно завантажена.");

try
{
    using (var scope = serviceProvider.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
       
        if (dbContext.Database.CanConnect())
        {
            Console.WriteLine("З'єднання з базою встановлено.");
        }
        else
        {
            Console.WriteLine("ПОМИЛКА ПIДКЛЮЧЕННЯ");
            return;
        }
    }
        
    var menu = serviceProvider.GetRequiredService<Menu>();
    menu.Run();
    Console.WriteLine("Натиснiть Enter, щоб продовжити");
    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine($"\n ПОМИЛКА: {ex.Message}");
}