using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Core.Services;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using LDBeauty.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void ShouldReturnCorrectUser()
        {

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
                Id = "176aa7c2-ba65-4a30-af66-1a412df21502",
                FirstName = "Venci",
                LastName = "Lazarov",
                Email = "vdl86@abv.bg",
            };

            var cart = new Cart()
            {
                TotalPrice = 0.0M,
                User = user,
                UserId = user.Id
            };

            var product = new Product()
            {
                Id = new Guid("176aa7c2-ba65-4a30-af66-1a412df21503"),
                ProductName = "Gen",
                CategoryId = 1,
                Description = "bbbb",
                MakeId = 2,
                Price = 5,
                ProductUrl = "url",
                Quantity = 10
            };


            await repo.AddAsync(user);
            await repo.AddAsync(cart);
            await repo.AddAsync(product);
            await repo.SaveChangesAsync();
        }
    }
}
