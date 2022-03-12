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
            Task<IEnumerable<GalleryCategoryViewModel>> model = galleryService.GetMCategories();

            return View(model);
        }

        public IActionResult GalleryTemp()
        {
            return View();
        }

        public IActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddImage(AddImageViewModel model)
        {
            var errors = new ErrorViewModel();

            errors = galleryService.AddImage(model);

            if (errors.ErrorMessages.Count == 0)
            {
                return RedirectToAction("AddImage");
            }

            return View("Error", errors);
        }
    }
}
