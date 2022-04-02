using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Gallery;
using LDBeauty.Infrastructure.Data;
using LDBeauty.Infrastructure.Data.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Services
{
    public class GalleryService : IGalleryService
    {
        private readonly ApplicationDbContext context;

        public GalleryService(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task AddImage(AddImageViewModel model)
        {

            ImgCategory category = await context.Set<ImgCategory>()
                .FirstOrDefaultAsync(c => c.CategoryName == model.Category);

            if (category == null)
            {
                category = new ImgCategory()
                {
                    CategoryName = model.Category,
                    ImgUrl = model.PictureUrl
                };

                context.Add(category);
            }

            Image image = new Image()
            {
                ImageUrl = model.PictureUrl,
                Description = model.Description,
                Category = category,
                CategoruId = category.Id
            };

            category.Images.Add(image);

            context.Add(image);
            await context.SaveChangesAsync();

        }

        public IEnumerable<ImageViewModel> AllImages()
        {
            return context.Set<Image>()
                .Select(i => new ImageViewModel()
                {
                    Id = i.Id,
                    ImgUrl = i.ImageUrl
                }).ToList();
        }

        public ImageDetailsViewModel GetImgDetails(int imageId)
        {
            return context.Set<Image>()
                .Where(i => i.Id == imageId)
                .Select(i => new ImageDetailsViewModel()
                {
                    ImgUrl = i.ImageUrl,
                    Description = i.Description,
                    CategoryName = i.Category.CategoryName,
                    Id = imageId
                }).FirstOrDefault();
        }

        public async Task AddToFavourites(string id, ApplicationUser user)
        {
             Image image = await context.Set<Image>()
                .FirstOrDefaultAsync(i => i.Id.ToString() == id);

            UserImage userImage = new UserImage()
            {
                ImageId = image.Id,
                Image = image,
                ApplicationUserId = user.Id,
                ApplicationUser = user
            };

            user.FavouriteImages.Add(image);

            await context.AddAsync(userImage);
            await context.SaveChangesAsync();
        }

        public IEnumerable<ImageViewModel> GetImages(int? categoryId)
        {
            return context.Set<Image>()
                .Where(i => i.CategoruId == categoryId)
                .Select(i => new ImageViewModel()
                {
                    Id = i.Id,
                    ImgUrl = i.ImageUrl
                }).ToList();
        }

        public IEnumerable<GalleryCategoryViewModel> GetMCategories()
        {

            var models = context
                .Set<ImgCategory>()
                .Select(c => new GalleryCategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.CategoryName,
                    ImgUrl = c.ImgUrl
                })
                .ToList();

            return models;
        }

        public async Task<IEnumerable<ImageViewModel>> GetFavouriteImages(ApplicationUser user)
        {
            return await context.Set<UserImage>()
                .Where(u => u.ApplicationUserId == user.Id)
                .Select(u => new ImageViewModel()
                {
                    ImgUrl = u.Image.ImageUrl,
                    Id = u.ImageId
                }).ToListAsync();
        }

        public async Task RemoveFromFavourite(int id, ApplicationUser user)
        {
            Image image = await context.Set<Image>()
                .FirstOrDefaultAsync(i => i.Id == id);

            UserImage userImage = await context.Set<UserImage>()
                .FirstOrDefaultAsync(i => i.ImageId == id &&
                i.ApplicationUserId == user.Id);

            user.FavouriteImages.Remove(image);
            context.Remove(userImage);

            await context.SaveChangesAsync();
        }
    }
}
