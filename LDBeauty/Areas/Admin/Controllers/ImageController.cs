using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
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

                ViewData[MessageConstant.ErrorMessage] = "Something went wrong!";
                return View();
            }

            ViewData[MessageConstant.SuccessMessage] = "Image was added successfuly";
            return View("/Admin/Image/AddImage");
        }
    }
}
