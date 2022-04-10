using LDBeauty.Core.Contracts;
using LDBeauty.Core.Services;
using LDBeauty.Infrastructure.Data.Identity;
using LDBeauty.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace LDBeauty.Test
{
    public class UserServiceTest
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
                .AddSingleton<IUserService, UserService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void ShouldReturnCorrectUserCount()
        {
            var service = serviceProvider.GetService<IUserService>();

            var user = service.GetAllUsers("ivan@");

            Assert.AreEqual(3, user.Result.Count());
        }

        [Test]
        public void ShouldReturnCorrectUserCountWithoutAdmin()
        {
            var service = serviceProvider.GetService<IUserService>();

            var user = service.GetAllUsers("a@abv.bg");

            Assert.AreEqual(2, user.Result.Count());
        }

        [Test]
        public void ShouldReturnCorrectUserByID()
        {
            var service = serviceProvider.GetService<IUserService>();

            var user = service.GetUserById("3").Result.Email;

            Assert.AreEqual("d@abv.bg", user);
        }

        [Test]
        public void ShouldReturnCorrectUserByUserName()
        {
            var service = serviceProvider.GetService<IUserService>();

            var user = service.GetUser("dog").Result.Email;

            Assert.AreEqual("d@abv.bg", user);
        }

        [Test]
        public void ShouldReturnCorrectUserByName()
        {
            var service = serviceProvider.GetService<IUserService>();

            var user = service.GetUSerByName("dog", 555).Result.Email;

            Assert.AreEqual("d@abv.bg", user);
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
                Email = "a@abv.bg",
                FirstName = "venci",
                LastName = "lazarov",
                Id = "1",
            };

            var user1 = new ApplicationUser()
            {
                Email = "v@abv.bg",
                FirstName = "ivan",
                LastName = "genov",
                Id = "2",
            };

            var user2 = new ApplicationUser()
            {
                Email = "d@abv.bg",
                UserName = "dog",
                FirstName = "stef",
                LastName = "toshev",
                Id = "3",
            };

            await repo.AddAsync(user);
            await repo.AddAsync(user1);
            await repo.AddAsync(user2);
            await repo.SaveChangesAsync();
        }
    }
}
