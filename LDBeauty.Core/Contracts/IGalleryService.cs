using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Gallery;
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
        ImageDetailsViewModel DetImgDetails(int imageId);
    }
}
