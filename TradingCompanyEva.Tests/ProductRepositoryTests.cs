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
    public class ProductRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductRepository _repository;
        private readonly IDbContextTransaction _transaction;

        public ProductRepositoryTests()
        {
            
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("TradingDBConnection"))
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new ProductRepository(_context);
            _transaction = _context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _context.Dispose();
        }

        

        [Fact]
        public void SearchByName_Should_FindProducts_WhenCalledWithExistingNamePart()
        {
            
            var category = new Category { CategoryName = "SearchCat", Description = "Desc" };
            _context.Categories.Add(category);
            _context.SaveChanges(); 

            var product = new Product
            {
                ProductName = "Унiкальна Книга Дюна",
                Price = 500,
                CategoryId = category.CategoryId, 
                StockQuantity = 10
            };
            _context.Products.Add(product);
            _context.SaveChanges();

            // Act
            var products = _repository.SearchByName("Унiкальна");

            // Assert
            Assert.NotNull(products);
            Assert.True(products.Any());
            Assert.Contains("Унiкальна", products.First().ProductName);
        }

        [Fact]
        public void GetByCategoryIdSorted_Should_SortByPriceDescending()
        {
            // Arrange
           
            var category = new Category { CategoryName = "SortCat", Description = "Desc" };
            _context.Categories.Add(category);
            _context.SaveChanges();

            
            _context.Products.Add(new Product
            {
                ProductName = "Cheap Product",
                Price = 100, 
                CategoryId = category.CategoryId,
                StockQuantity = 5
            });

            _context.Products.Add(new Product
            {
                ProductName = "Expensive Product",
                Price = 900, 
                CategoryId = category.CategoryId,
                StockQuantity = 5
            });
            _context.SaveChanges();

            // Act
            
            var products = _repository.GetByCategoryIdSorted(category.CategoryId, "Price", ascending: false);

            // Assert
            Assert.Equal(2, products.Count());
            
            Assert.Equal("Expensive Product", products.First().ProductName);
            Assert.Equal(900, products.First().Price);
        }

        [Fact]
        public void Add_Should_AddNewProduct_And_RollbackShouldRemoveIt()
        {
            // Arrange
            
            var category = new Category { CategoryName = "AddTestCat", Description = "Desc" };
            _context.Categories.Add(category);
            _context.SaveChanges();

            var newProduct = new Product
            {
                ProductName = "New Test Product",
                Price = 99,
                CategoryId = category.CategoryId, 
                StockQuantity = 10
            };

            // Act
            _repository.Add(newProduct);

            // Assert
            var productFromDb = _context.Products.FirstOrDefault(p => p.ProductName == "New Test Product");
            Assert.NotNull(productFromDb);
            Assert.Equal(99, productFromDb.Price);
            Assert.Equal(category.CategoryId, productFromDb.CategoryId);
        }
    }
}