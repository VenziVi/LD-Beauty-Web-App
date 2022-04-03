using LDBeauty.Core.Constants;
using LDBeauty.Core.Constraints;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Helpers;
using LDBeauty.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Areas.Admin.Controllers
{
    public class ImageController : BaseController
    {
        private readonly IGalleryService galleryService;

        public ImageController(IGalleryService _galleryService)
        {
            galleryService = _galleryService;
        }

        public IActionResult AddImage()
        {
            if (ImageAdding.IsAdded)
            {
                ViewData[MessageConstant.SuccessMessage] = "Image was added successfuly";
                ImageAdding.IsAdded = false;
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddImage(AddImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData[MessageConstant.ErrorMessage] = "Data is not correct!";
                return View();
            }

            try
            {
                await galleryService.AddImage(model);
            }
            catch (Exception)
            {
                ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
                return View("_Error", error);
            }

            ImageAdding.IsAdded = true;
            return Redirect("AddImage");
        }
    }
}
