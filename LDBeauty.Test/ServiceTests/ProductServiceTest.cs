using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Product;
using LDBeauty.Core.Services;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using LDBeauty.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LDBeauty.Test.ServiceTests
{
    public class ProductServiceTest
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
                .AddSingleton<IProductService, ProductService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void ShouldReturnCountOnGetAllProducts()
        {
            var service = serviceProvider.GetService<IProductService>();

            var products = service.GetAllProducts().Result;

            Assert.AreEqual(2, products.Count());
        }

        [Test]
        public void ShouldReturnProductCountByCategory()
        {

            var service = serviceProvider.GetService<IProductService>();

            var product = service.GetProductsByCategory(1).Result.Count();

            Assert.AreEqual(1, product);   
        }

        [Test]
        public void ShouldReturnNullProductByCategory()
        {

            var service = serviceProvider.GetService<IProductService>();

            var product = service.GetProductsByCategory(555).Result.Count();

            Assert.AreEqual(0, product);
        }

        [Test]
        public void ShouldReturnCorrectProduct()
        {

            var service = serviceProvider.GetService<IProductService>();

            var product = service.GetProduct(2).Result;

            Assert.AreEqual("some", product.Description);
        }

        [Test]
        public void ShouldReturnProductCountByMake()
        {

            var service = serviceProvider.GetService<IProductService>();

            var product = service.GetProductsByMake(2).Result.Count();

            Assert.AreEqual(1, product);
        }

        [Test]
        public void ShouldReturnNullProductByMake()
        {

            var service = serviceProvider.GetService<IProductService>();

            var product = service.GetProductsByMake(555).Result.Count();

            Assert.AreEqual(0, product);
        }

        [Test]
        public void ShouldReturnProductCountByName()
        {

            var service = serviceProvider.GetService<IProductService>();

            var product = service.GetProductsByName("VVVV").Result.Count();

            Assert.AreEqual(2, product);
        }

        [Test]
        public void ShouldReturnNullForNonProductByName()
        {

            var service = serviceProvider.GetService<IProductService>();

            var product = service.GetProductsByName("ZZZZ").Result.Count();

            Assert.AreEqual(0, product);
        }

        [Test]
        public void ShouldIncreaseCountOfProductsOnAddProduct()
        {
            AddProductViewModel model = new AddProductViewModel()
            {
                ProductName = "Name",
                ProductUrl = "url",
                Price = 55.00M,
                Description = "des",
                Quantity = 3,
                Category = "new",
                Make = "new"
            };

            var service = serviceProvider.GetService<IProductService>();

            service.AddProduct(model);

            var products = service.GetAllProducts().Result;

            Assert.AreEqual(3, products.Count());
        }

        [Test]
        public void ShouldIncreaseFavouriteCountAndReturnCorect()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                Email = "vdl86@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
            };

            var service = serviceProvider.GetService<IProductService>();

            service.AddToFavourites(1, user);

            Assert.AreEqual(1, user.FavouriteProducts.Count);

            var product = service.GetFavouriteProducts(user).Result;

            Assert.AreEqual("VVVV", product.FirstOrDefault().ProductName);

            service.RemoveFromFavourite(1, user);

            Assert.AreEqual(0, user.FavouriteProducts.Count);
        }

        [Test]
        public void ShouldReturnQuantityAfterEditProduct()
        {
            AddProductViewModel model = new AddProductViewModel()
            {
                ProductName = "Name",
                ProductUrl = "url",
                Price = 55.00M,
                Description = "dessss",
                Quantity = 3,
                Category = "new",
                Make = "new"
            };

            var service = serviceProvider.GetService<IProductService>();

            service.EditProduct(model, 1);

            var product = service.GetProduct(1);

            Assert.AreEqual(3, product.Result.Quantity);
        }

        [Test]
        public void ShouldThrowExOnAddProductToFavoutites()
        {
            var user2 = new ApplicationUser()
            {
                Email = "d@abv.bg",
                UserName = "dog",
                FirstName = "stef",
                LastName = "toshev",
                FavouriteProducts = new List<Product>(),
                Id = "3",
            };

            var service = serviceProvider.GetService<IProductService>();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.AddToFavourites(66, user2), ErrorMessages.ProductNotFound);
        }

        [Test]
        public void ShouldNotThrowExOnAddProductToFavoutites()
        {
            var user2 = new ApplicationUser()
            {
                Email = "d@abv.bg",
                UserName = "dog",
                FirstName = "stef",
                LastName = "toshev",
                FavouriteProducts = new List<Product>(),
                Id = "3",
            };

            var service = serviceProvider.GetService<IProductService>();

            Assert.DoesNotThrowAsync(async () => await service.AddToFavourites(1, user2));
        }

        [Test]
        public void ShouldThrowExOnRemoveProductToFavoutites()
        {
            var user2 = new ApplicationUser()
            {
                Email = "d@abv.bg",
                UserName = "dog",
                FirstName = "stef",
                LastName = "toshev",
                FavouriteProducts = new List<Product>(),
                Id = "3",
            };

            var service = serviceProvider.GetService<IProductService>();

            Assert.ThrowsAsync<ArgumentException>(async () => await service.RemoveFromFavourite(66, user2), ErrorMessages.ProductNotFound);
        }


        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }

        private async Task SeedDbAsync(IApplicationDbRepository repo)
        {

            Category category = new Category()
            {
                Id = 1,
                CategoryName = "AAA"
            };


            Category category1 = new Category()
            {
                Id = 2,
                CategoryName = "AAA"
            };

            Make make = new Make()
            {
                Id = 1,
                MakeName = "bbb"
            };

            Make make1 = new Make()
            {
                Id = 2,
                MakeName = "bbb"
            };


            var product = new Product()
            {
                Id = 1,
                Category = category,
                CategoryId = 1,
                Description = "dessss",
                Make = make,
                MakeId = 1,
                Price = 5.00M,
                ProductName = "VVVV",
                ProductUrl = "url",
                Quantity = 5
            };

            var product1 = new Product()
            {
                Id = 2,
                Category = category1,
                CategoryId = 2,
                Description = "some",
                Make = make1,
                MakeId = 2,
                Price = 5.00M,
                ProductName = "VVVV",
                ProductUrl = "url",
                Quantity = 5,
            };


            await repo.AddAsync(category);
            await repo.AddAsync(make);
            await repo.AddAsync(product);
            await repo.AddAsync(category1);
            await repo.AddAsync(make1);
            await repo.AddAsync(product1);
            await repo.SaveChangesAsync();
        }
    }
}
