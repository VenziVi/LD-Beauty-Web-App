using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Gallery;
using LDBeauty.Infrastructure.Data;
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

            try
            {
                context.Add(image);
                await context.SaveChangesAsync();

            }
            catch (Exception)
            {
            }

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

        public ImageDetailsViewModel DetImgDetails(int imageId)
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
    }
}
