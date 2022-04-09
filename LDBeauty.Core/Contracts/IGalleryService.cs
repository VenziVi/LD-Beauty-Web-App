using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Gallery;
using LDBeauty.Infrastructure.Data.Identity;

namespace LDBeauty.Core.Contracts
{
    public interface IGalleryService
    {
        Task AddImage(AddImageViewModel model);
        Task<List<GalleryCategoryViewModel>> GetCategories();
        Task<List<ImageViewModel>> AllImages();
        Task<List<ImageViewModel>> GetImages(int? categoryId);
        Task<ImageDetailsViewModel> GetImgDetails(int imageId);
        Task AddToFavourites(string id, ApplicationUser user);
        Task<List<ImageViewModel>> GetFavouriteImages(ApplicationUser user);
        Task RemoveFromFavourite(int id, ApplicationUser user);
    }
}
