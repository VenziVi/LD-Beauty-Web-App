using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Core.Services;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using LDBeauty.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LDBeauty.Test.ServiceTests
{
    public class OrderServiceTest
    {
        private ServiceProvider serviceProvider;
        private InMemoryDbContext dbContext;

        [SetUp]
        public async Task Setup()
        {
            dbContext = new InMemoryDbContext();
            var serviceCollection = new ServiceCollection();

            serviceProvider = serviceCollection
                .AddSingleton(sp => dbContext.CreateContext())
                .AddSingleton<IApplicationDbRepository, ApplicationDbRepository>()
                .AddSingleton<IOrderService, OrderService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void ShouldThrowIfCardIsNull()
        {
            FinishOrderViewModel model = new FinishOrderViewModel()
            {
                UserId = "1",
                CartId = "55",
                Address = "address",
                Email = "v@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
                Phone = "1234567899"
            };

            var service = serviceProvider.GetService<IOrderService>();

            Assert.CatchAsync<ArgumentException>(async () => await service.FinishOrder(model), ErrorMessages.SomethingWrong);
        }

        [Test]
        public void ShouldThrowIfNoEnoughtQuantity()
        {
            FinishOrderViewModel model = new FinishOrderViewModel()
            {
                UserId = "1",
                CartId = "1",
                Address = "address",
                Email = "v@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
                Phone = "1234567899"
            };

            var service = serviceProvider.GetService<IOrderService>();

            Assert.CatchAsync<ArgumentException>(async () => await service.FinishOrder(model));
        }


        [Test]
        public void ShouldThrowIfProductListIsNull()
        {
            FinishOrderViewModel model = new FinishOrderViewModel()
            {
                UserId = "1",
                CartId = "1",
                Address = "address",
                Email = "v@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
                Phone = "1234567899"
            };

            var service = serviceProvider.GetService<IOrderService>();

            Assert.CatchAsync<ArgumentException>(async () => await service.FinishOrder(model), ErrorMessages.SomethingWrong);
        }

        [Test]
        public void ShouldBeNullWhenNoOrder()
        {
            var service = serviceProvider.GetService<IOrderService>();

            var order = service.GetUserProducts("3").Result;

            Assert.AreEqual(0, order.Count());
        }

        [Test]
        public void ShouldNotBeNullWhenOrderFinished()
        {
            FinishOrderViewModel model = new FinishOrderViewModel()
            {
                UserId = "2",
                CartId = "2",
                Address = "address",
                Email = "v@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
                Phone = "1234567899"
            };

            var service = serviceProvider.GetService<IOrderService>();

            service.FinishOrder(model);

            var order = service.GetUserProducts("2").Result;

            Assert.AreEqual(1, order.Count());
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }


        private async Task SeedDbAsync(IApplicationDbRepository repo)
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                FirstName = "Venci",
                LastName = "Lazarov",
                Email = "v86@abv.bg",
                UserName = "dog",
            };

            var cart = new Cart()
            {
                Id = 1,
                User = user,
                UserId = "1",
                IsDeleted = false
            };

            var product = new Product()
            {
                Id = 11,
                Category = new Category()
                {
                    Id = 1,
                    CategoryName = "AAA"
                },
                CategoryId = 1,
                Description = "dessss",
                Make = new Make()
                {
                    Id = 1,
                    MakeName = "bbb"
                },
                MakeId = 1,
                Price = 5.00M,
                ProductName = "VVVV",
                ProductUrl = "url",
                Quantity = 3
            };

            var productList = new AddedProduct()
            {
                Cart = cart,
                CartId = 1,
                Id = 8,
                Product = product,
                ProductId = 11,
                Quantity = 5,
            };

            var user1 = new ApplicationUser()
            {
                Id = "2",
                FirstName = "Venci",
                LastName = "Lazarov",
                Email = "v8226@abv.bg",
                UserName = "cat",
            };

            var cart1 = new Cart()
            {
                Id = 2,
                User = user1,
                UserId = "2",
                IsDeleted = false
            };

            var product1 = new Product()
            {
                Id = 12,
                Category = new Category()
                {
                    Id = 2,
                    CategoryName = "bbb"
                },
                CategoryId = 2,
                Description = "some",
                Make = new Make()
                {
                    Id = 2,
                    MakeName = "bbbb"
                },
                MakeId = 2,
                Price = 5.00M,
                ProductName = "VVVV",
                ProductUrl = "url",
                Quantity = 5
            };

            var productList1 = new AddedProduct()
            {
                Id = 9,
                Cart = cart1,
                CartId = 2,
                Product = product1,
                ProductId = 12,
                Quantity = 2,
            };

            var user2 = new ApplicationUser()
            {
                Id = "3",
                FirstName = "Venci",
                LastName = "Lazarov",
                Email = "v82236@abv.bg",
                UserName = "cat",
            };

            await repo.AddAsync(user);
            await repo.AddAsync(cart);
            await repo.AddAsync(product);
            await repo.AddAsync(productList);
            await repo.AddAsync(user1);
            await repo.AddAsync(cart1);
            await repo.AddAsync(product1);
            await repo.AddAsync(productList1);
            await repo.AddAsync(user2);
            await repo.SaveChangesAsync();
        }
    }
}
