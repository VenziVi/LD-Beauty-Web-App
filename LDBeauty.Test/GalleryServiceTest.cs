using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Gallery;
using LDBeauty.Core.Services;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using LDBeauty.Infrastructure.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace LDBeauty.Test
{
    public class GalleryerviceTest
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
                .AddSingleton<IGalleryService, GalleryService>()
                .BuildServiceProvider();

            var repo = serviceProvider.GetService<IApplicationDbRepository>();
            await SeedDbAsync(repo);
        }

        [Test]
        public void ShouldReturnCategory()
        {
            var service = serviceProvider.GetService<IGalleryService>();

            Assert.IsNotNull(async () => await service.GetCategories());
        }

        [Test]
        public void ShouldReturnCategoryCount()
        {
            var service = serviceProvider.GetService<IGalleryService>();

            var categories = service.GetCategories();

            Assert.AreEqual(2, categories.Result.Count);
        }

        [Test]
        public void ShouldReturnNumberOfImages()
        {
            var service = serviceProvider.GetService<IGalleryService>();

            var images = service.AllImages();

            Assert.AreEqual(2, images.Result.Count);
        }

        [Test]
        public void ShouldNotReturnCurrentImageDetails()
        {
            var image = new ImageDetailsViewModel()
            {
                CategoryName = "Wedding",
                Id = 1,
                Description = "aaaaaaaa",
                ImgUrl = "url"
            };

            var service = serviceProvider.GetService<IGalleryService>();

            var img = service.GetImgDetails(2).Result.Description;

            Assert.AreNotEqual(image.Description, img);
        }

        [Test]
        public void ShouldReturnCurrImageDetails()
        {
            var image1 = new Image()
            {
                Id = 2,
                CategoruId = 1,
                Description = "bbbbbbbb",
                ImageUrl = "url"
            };

            var service = serviceProvider.GetService<IGalleryService>();

            var img = service.GetImgDetails(2).Result.Description;

            Assert.AreEqual(image1.Description, img);
        }

        [Test]
        public void ShouldReturnImageByCategoryId()
        {
            var service = serviceProvider.GetService<IGalleryService>();

            var img = service.GetImages(1).Result.Count;

            Assert.AreEqual(2, img);
        }

        [Test]
        public void ShouldReturnZeroForMissingCategory()
        {
            var service = serviceProvider.GetService<IGalleryService>();

            var img = service.GetImages(8).Result.Count;

            Assert.AreEqual(0, img);
        }

        [Test]
        public void ShouldAddImagetoFavourites()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                Email = "vdl86@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
            };

            var service = serviceProvider.GetService<IGalleryService>();

            var result = service.AddToFavourites("1", user);

            Assert.AreEqual(1, user.FavouriteImages.Count);
        }

        [Test]
        public void ShouldReturnFavouriteImages()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                Email = "vdl86@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
            };

            var service = serviceProvider.GetService<IGalleryService>();

            var result = service.GetFavouriteImages(user);

            Assert.AreEqual(1, result.Result.Count);
        }

        [Test]
        public void ShouldThrowExceptionWhenImageIsNull()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                Email = "vdl86@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
            };

            var service = serviceProvider.GetService<IGalleryService>();

            Assert.CatchAsync<ArgumentException>(async () => await service.AddToFavourites("22", user), "Image not found, please try again later.");
        }

        [Test]
        public void ShouldRemoveFavouriteImages()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                Email = "vdl86@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
            };

            var service = serviceProvider.GetService<IGalleryService>();

            service.RemoveFromFavourite(1, user);

            Assert.AreEqual(0, user.FavouriteImages.Count);
        }

        [Test]
        public void ShouldIncreaseImagesAndCategoryCountOnAdd()
        {
            var service = serviceProvider.GetService<IGalleryService>();

            service.AddImage(new AddImageViewModel()
            {
                Category = "new",
                Description = "sssss",
                PictureUrl = "newUrl"
            });

            var img = service.AllImages();
            var cat = service.GetCategories();

            Assert.AreEqual(3, img.Result.Count);
            Assert.AreEqual(3, cat.Result.Count);
        }

        [Test]
        public void ShouldThrowExceptionWhenImageIsNullOnRemoveImage()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                Email = "vdl86@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
            };

            var service = serviceProvider.GetService<IGalleryService>();

            Assert.CatchAsync<ArgumentException>(async () => await service.RemoveFromFavourite(222, user), "Image not found, please try again later.");
        }

        [Test]
        public void ShouldThrowExceptionWhenUserImageIsNull()
        {
            var user = new ApplicationUser()
            {
                Id = "1",
                Email = "vdl86@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
            };

            var service = serviceProvider.GetService<IGalleryService>();

            Assert.CatchAsync<ArgumentException>(async () => await service.RemoveFromFavourite(22, user), "Something went wrong, please try again later.");
        }



        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
        }


        private async Task SeedDbAsync(IApplicationDbRepository repo)
        {
            var category = new ImgCategory()
            {
                Id = 1,
                CategoryName = "Wedding",
                ImgUrl = "url"
            };

            var category1 = new ImgCategory()
            {
                Id = 2,
                CategoryName = "Color",
                ImgUrl = "url"
            };

            var image = new Image()
            {
                Id = 1,
                CategoruId = 1,
                Description = "aaaaaaaa",
                ImageUrl = "url"
            };

            var image1 = new Image()
            {
                Id = 2,
                CategoruId = 1,
                Description = "bbbbbbbb",
                ImageUrl = "url"
            };

            var user = new ApplicationUser()
            {
                Id = "1",
                Email = "vdl86@abv.bg",
                FirstName = "Venci",
                LastName = "Lazarov",
            };

            var userImg = new UserImage()
            {
                ApplicationUser = user,
                ApplicationUserId = "1",
                Image = image1,
                ImageId = 2
            };


            await repo.AddAsync(category);
            await repo.AddAsync(category1);
            await repo.AddAsync(image);
            await repo.AddAsync(image1);
            await repo.AddAsync(user);
            await repo.AddAsync(userImg);
            await repo.SaveChangesAsync();
        }
    }
}