using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using System.IO;
using TradingCompanyEva.DAL.EF;
using TradingCompanyEva.DAL.EF.Repositories;
using TradingCompanyEva.Domain.Entites;
using TradingCompanyEva.Domain.Interfaces;
using Xunit;
    

namespace TradingCompanyEva.Tests
{
    public class CartRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartRepository _repository;
        private readonly IDbContextTransaction _transaction;

        public CartRepositoryTests()
        {
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("TradingDBConnection"))
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new CartRepository(_context);

            _transaction = _context.Database.BeginTransaction();
        }

        public void Dispose()
        {
           
            _transaction.Rollback();
            _transaction.Dispose();
            _context.Dispose();
        }

        [Fact]
        public void GetByUserId_Should_ReturnCart_WhenExists()
        {
            
            var user = new User { Username = "CartTestUser", PasswordHash = "hash", Email = "cart@test.com" };
            _context.Users.Add(user);
            _context.SaveChanges(); 

            var cart = new Cart { UserId = user.UserId, CreatedAt = DateTime.UtcNow };
            _context.Carts.Add(cart);
            _context.SaveChanges();

            
            var result = _repository.GetByUserId(user.UserId);

            Assert.NotNull(result);
            Assert.Equal(cart.CartId, result.CartId);
            Assert.Equal(user.UserId, result.UserId);
        }

        [Fact]
        public void AddItemToCart_Should_AddNewItem_WhenItemDoesNotExist()
        {
            
            var category = new Category { CategoryName = "TestCat", Description = "Desc" };
            _context.Categories.Add(category);
            _context.SaveChanges();

            var product = new Product { ProductName = "TestProd", Price = 100, StockQuantity = 10, CategoryId = category.CategoryId };
            _context.Products.Add(product);

            
            var user = new User { Username = "CartAddUser", PasswordHash = "hash" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var cart = new Cart { UserId = user.UserId, CreatedAt = DateTime.UtcNow };
            _context.Carts.Add(cart);
            _context.SaveChanges();

            
            _repository.AddItemToCart(cart.CartId, product.ProductId, 5);

            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == product.ProductId);
            Assert.NotNull(cartItem);
            Assert.Equal(5, cartItem.Quantity);
        }

        [Fact]
        public void AddItemToCart_Should_IncreaseQuantity_WhenItemAlreadyExists()
        {
            
            var category = new Category { CategoryName = "TestCat2", Description = "Desc" };
            _context.Categories.Add(category);
            _context.SaveChanges();

            var product = new Product { ProductName = "TestProd2", Price = 100, StockQuantity = 10, CategoryId = category.CategoryId };
            _context.Products.Add(product);

            var user = new User { Username = "CartUpdateUser", PasswordHash = "hash" };
            _context.Users.Add(user);
            _context.SaveChanges();

            var cart = new Cart { UserId = user.UserId, CreatedAt = DateTime.UtcNow };
            _context.Carts.Add(cart);
            _context.SaveChanges();

           
            _context.CartItems.Add(new CartItem { CartId = cart.CartId, ProductId = product.ProductId, Quantity = 2 });
            _context.SaveChanges();

           
            _repository.AddItemToCart(cart.CartId, product.ProductId, 3);

            
            var cartItem = _context.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == product.ProductId);
            Assert.NotNull(cartItem);
            Assert.Equal(5, cartItem.Quantity); 
        }
    }
}