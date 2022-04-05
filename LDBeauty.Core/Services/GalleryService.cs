using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Gallery;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using LDBeauty.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LDBeauty.Core.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly IApplicationDbRepository repo;

        public GalleryService(IApplicationDbRepository _repo)
        {
            repo = _repo;
        }

        public async Task AddImage(AddImageViewModel model)
        {

            ImgCategory category = await repo.All<ImgCategory>()
                .FirstOrDefaultAsync(c => c.CategoryName == model.Category);

            if (category == null)
            {
                category = new ImgCategory()
                {
                    CategoryName = model.Category,
                    ImgUrl = model.PictureUrl
                };

                await repo.AddAsync(category);
            }

            Image image = new Image()
            {
                ImageUrl = model.PictureUrl,
                Description = model.Description,
                Category = category,
                CategoruId = category.Id
            };

            category.Images.Add(image);

            await repo.AddAsync(image);
            await repo.SaveChangesAsync();

        }

        public async Task<List<ImageViewModel>> AllImages()
        {
            return await repo.All<Image>()
                .Select(i => new ImageViewModel()
                {
                    Id = i.Id,
                    ImgUrl = i.ImageUrl
                }).ToListAsync();
        }

        public async Task<ImageDetailsViewModel> GetImgDetails(int imageId)
        {
            return await repo.All<Image>()
                .Where(i => i.Id == imageId)
                .Select(i => new ImageDetailsViewModel()
                {
                    ImgUrl = i.ImageUrl,
                    Description = i.Description,
                    CategoryName = i.Category.CategoryName,
                    Id = imageId
                }).FirstOrDefaultAsync();
        }

        public async Task AddToFavourites(string id, ApplicationUser user)
        {
             Image image = await repo.All<Image>()
                .FirstOrDefaultAsync(i => i.Id.ToString() == id);

            UserImage userImage = new UserImage()
            {
                ImageId = image.Id,
                Image = image,
                ApplicationUserId = user.Id,
                ApplicationUser = user
            };

            user.FavouriteImages.Add(image);

            await repo.AddAsync(userImage);
            await repo.SaveChangesAsync();
        }

        public async Task<List<ImageViewModel>> GetImages(int? categoryId)
        {
            return await repo.All<Image>()
                .Where(i => i.CategoruId == categoryId)
                .Select(i => new ImageViewModel()
                {
                    Id = i.Id,
                    ImgUrl = i.ImageUrl
                }).ToListAsync();
        }

        public async Task<List<GalleryCategoryViewModel>> GetCategories()
        {

            var models = await repo.All<ImgCategory>()
                .Select(c => new GalleryCategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.CategoryName,
                    ImgUrl = c.ImgUrl
                })
                .ToListAsync();

            return models;
        }

        public async Task<List<ImageViewModel>> GetFavouriteImages(ApplicationUser user)
        {
            return await repo.All<UserImage>()
                .Where(u => u.ApplicationUserId == user.Id)
                .Select(u => new ImageViewModel()
                {
                    ImgUrl = u.Image.ImageUrl,
                    Id = u.ImageId
                }).ToListAsync();
        }

        public async Task RemoveFromFavourite(int id, ApplicationUser user)
        {
            Image image = await repo.All<Image>()
                .FirstOrDefaultAsync(i => i.Id == id);

            UserImage userImage = await repo.All<UserImage>()
                .FirstOrDefaultAsync(i => i.ImageId == id &&
                i.ApplicationUserId == user.Id);

            user.FavouriteImages.Remove(image);
            repo.Delete<UserImage>(userImage);

            await repo.SaveChangesAsync();
        }
    }
}
