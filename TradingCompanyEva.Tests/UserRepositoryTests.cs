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
    public class UserRepositoryTests : IDisposable
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserRepository _repository;
        private readonly IDbContextTransaction _transaction;

        public UserRepositoryTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("TradingDBConnection"))
                .Options;

            _context = new ApplicationDbContext(options);
            _repository = new UserRepository(_context);
            _transaction = _context.Database.BeginTransaction();
        }

        public void Dispose()
        {
            _transaction.Rollback();
            _transaction.Dispose();
            _context.Dispose();
        }

        [Fact]
        public void Add_Should_CreateNewUser()
        {
            // Arrange
            var newUser = new User
            {
                Username = "TestUser_Generic",
                PasswordHash = "hashed_pass",
                Email = "test@generic.com"
            };

            // Act
            _repository.Add(newUser);

            // Assert
            var userFromDb = _context.Users.FirstOrDefault(u => u.Username == "TestUser_Generic");
            Assert.NotNull(userFromDb);
            Assert.Equal("test@generic.com", userFromDb.Email);
        }

        [Fact]
        public void GetByUsername_Should_ReturnUserWithRoles()
        {
            // Arrange
            
            var role = new Role { RoleName = "TestRole" };
            _context.Roles.Add(role);
            _context.SaveChanges();

           
            var user = new User { Username = "LoginUserTest", PasswordHash = "123" };
            _context.Users.Add(user);
            _context.SaveChanges();

           
            _context.UserRoles.Add(new UserRole { UserId = user.UserId, RoleId = role.RoleId });
            _context.SaveChanges();

            // Act
            var result = _repository.GetByUsername("LoginUserTest");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("LoginUserTest", result.Username);

            
            Assert.NotNull(result.UserRoles);
            Assert.NotEmpty(result.UserRoles);
            Assert.Equal("TestRole", result.UserRoles.First().Role.RoleName);
        }
    }
}