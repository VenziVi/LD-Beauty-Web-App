using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Gallery;
using LDBeauty.Infrastructure.Data.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Contracts
{
    public interface IGalleryService
    {
        Task AddImage(AddImageViewModel model);
        IEnumerable<GalleryCategoryViewModel> GetMCategories();
        IEnumerable<ImageViewModel> AllImages();
        IEnumerable<ImageViewModel> GetImages(int? categoryId);
        ImageDetailsViewModel GetImgDetails(int imageId);
        Task AddToFavourites(string id, ApplicationUser user);
        Task<IEnumerable<ImageViewModel>> GetFavouriteImages(ApplicationUser user);
        Task RemoveFromFavourite(int id, ApplicationUser user);
    }
}
