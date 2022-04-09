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

namespace LDBeauty.Test
{
    public class CartServiceTest
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
                .AddSingleton<ICartService, CartService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void ShouldReturnNullWhenCartIsDeleted()
        {
            var service = serviceProvider.GetService<ICartService>();
            var cart = service.GetCart("dog");
            Assert.AreEqual(null, cart.Result);
        }

        [Test]
        public void ShouldReturnCorrectProductList()
        {
            var service = serviceProvider.GetService<ICartService>();
            var cart = service.GetCart("cat");
            Assert.AreEqual("VVVV", cart.Result.Products.FirstOrDefault().ProductName);
        }

        [Test]
        public void ShouldReturnCorrectCartTotalPrice()
        {
            var service = serviceProvider.GetService<ICartService>();
            var cart = service.GetCart("cat");
            Assert.AreEqual("15.00", cart.Result.Products.FirstOrDefault().Price);
        }

        [Test]
        public void ShouldRemoveProductFromCart()
        {
            var service = serviceProvider.GetService<ICartService>();

            service.DeleteProduct(11, "cat");
            var cart = service.GetCart("cat");

            Assert.AreEqual(0, cart.Result.Products.Count);
        }

        [Test]
        public void ShouldCreateNewCart()
        {
            AddToCartViewModel model = new AddToCartViewModel()
            {
                ProductId = 12,
                Quantity = 2
            };

            var service = serviceProvider.GetService<ICartService>();

            service.AddToCart(model, "mouse");
            var cart = service.GetCart("mouse");

            Assert.AreEqual(1, cart.Result.Products.Count);
        }

        [Test]
        public void ShouldThrowIfProductIsNull()
        {
            AddToCartViewModel model = new AddToCartViewModel()
            {
                ProductId = 222,
                Quantity = 2
            };

            var service = serviceProvider.GetService<ICartService>();

            Assert.CatchAsync<ArgumentException>(async () => await service.AddToCart(model, "mouse"), ErrorMessages.ProductNotFound);
        }

        [Test]
        public void ShouldIncreaseProductCountOnAddToCart()
        {
            AddToCartViewModel model = new AddToCartViewModel()
            {
                ProductId = 12,
                Quantity = 2
            };

            var service = serviceProvider.GetService<ICartService>();

            service.AddToCart(model, "cat");
            var cart = service.GetCart("cat");

            Assert.AreEqual(2, cart.Result.Products.Count);
        }

        [Test]
        public void ShouldReturnCorrectTotalPriceOnAddToCart()
        {
            AddToCartViewModel model = new AddToCartViewModel()
            {
                ProductId = 12,
                Quantity = 2
            };

            var service = serviceProvider.GetService<ICartService>();

            service.AddToCart(model, "cat");
            var cart = service.GetCart("cat");

            Assert.AreEqual(19.00M, cart.Result.TotalPrice);
        }

        [Test]
        public void ShouldReturnCorrectTotalPriceAfterDeleteProduct()
        {
            AddToCartViewModel model = new AddToCartViewModel()
            {
                ProductId = 12,
                Quantity = 2
            };

            var service = serviceProvider.GetService<ICartService>();

            service.AddToCart(model, "cat");
            var cart = service.GetCart("cat");

            Assert.AreEqual(19.00M, cart.Result.TotalPrice);

            service.DeleteProduct(12, "cat");
            var cart2 = service.GetCart("cat");

            Assert.AreEqual(15.00M, cart2.Result.TotalPrice);
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
                Id = "5",
                FirstName = "Venci",
                LastName = "Lazarov",
                Email = "v86@abv.bg",
                UserName = "dog",
            };

            var cart = new Cart()
            {
                Id = 1,
                User = user,
                UserId = "5",
                IsDeleted = true
            };

            var user1 = new ApplicationUser()
            {
                Id = "6",
                FirstName = "Venci",
                LastName = "Lazarov",
                Email = "vdd86@abv.bg",
                UserName = "cat",
            };

            var cart1 = new Cart()
            {
                Id = 2,
                User = user1,
                UserId = "6",
                IsDeleted = false
            };

            var user2 = new ApplicationUser()
            {
                Id = "9",
                FirstName = "Venci",
                LastName = "Lazarov",
                Email = "v8s6@abv.bg",
                UserName = "mouse",
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
                Quantity = 5
            };

            var product1 = new Product()
            {
                Id = 12,
                Category = new Category()
                {
                    Id = 2,
                    CategoryName = "CCC"
                },
                CategoryId = 1,
                Description = "reeee",
                Make = new Make()
                {
                    Id = 2,
                    MakeName = "ggg"
                },
                MakeId = 1,
                Price = 2.00M,
                ProductName = "MMM",
                ProductUrl = "url",
                Quantity = 6
            };

            var productList = new AddedProduct()
            {
                Cart = cart1,
                CartId = 2,
                Id = 8,
                Product = product,
                ProductId = 11,
                Quantity = 3,                
            };

            await repo.AddAsync(user);
            await repo.AddAsync(user1);
            await repo.AddAsync(cart);
            await repo.AddAsync(cart1);
            await repo.AddAsync(product);
            await repo.AddAsync(product1);
            await repo.AddAsync(user2);
            await repo.AddAsync(productList);
            await repo.SaveChangesAsync();
        }
    }
}
