using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Gallery;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IGalleryService galleryService;

        public GalleryController(IGalleryService _galleryService)
        {
            galleryService = _galleryService;
        }

        public IActionResult Category()
        {
            IEnumerable<GalleryCategoryViewModel> model = galleryService.GetMCategories();

            return View(model);
        }

        public IActionResult Images(int? categoryId)
        {
            if (categoryId == null)
            {
                IEnumerable<ImageViewModel> images = galleryService.AllImages();

                return View(images);
            }

            IEnumerable<ImageViewModel> currImages = galleryService.GetImages(categoryId);

            return View(currImages);
        }

        public IActionResult Details(int imageId)
        {
            ImageDetailsViewModel imageDetails = galleryService.DetImgDetails(imageId);

            return View(imageDetails);
        }

    }
}
