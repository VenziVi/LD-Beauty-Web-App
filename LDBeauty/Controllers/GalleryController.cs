using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Gallery;
using LDBeauty.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IGalleryService galleryService;
        private readonly IUserService userService;
        private readonly ILogger<GalleryController> logger;

        public GalleryController(
            IGalleryService _galleryService,
            IUserService _userService,
            ILogger<GalleryController> _logger)
        {
            galleryService = _galleryService;
            userService = _userService;
            logger = _logger;
        }

        public async Task<IActionResult> Category()
        {
            List<GalleryCategoryViewModel> model = null;

            try
            {
                model = await galleryService.GetCategories();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GalleryController/Category");
                return DatabaseError();
            }

            return View(model);
        }

        public async Task<IActionResult> Images(int? id)
        {
            List<ImageViewModel> images = null;

            if (id == null)
            {
                try
                {
                    images = await galleryService.AllImages();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "GalleryController/Images");
                    return DatabaseError();
                }
            }
            else
            {
                try
                {
                    images = await galleryService.GetImages(id);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "GalleryController/Images");
                    return DatabaseError();
                }
            }

            return View(images);
        }

        public async Task<IActionResult> Details(int id)
        {
            ImageDetailsViewModel imageDetails = null;

            try
            {
                imageDetails = await galleryService.GetImgDetails(id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GalleryController/Details");
                return DatabaseError();
            }

            return View(imageDetails);
        }

        [Authorize]
        public async Task<IActionResult> AddImageToFavourites(string id)
        {
            var userName = User.Identity.Name;
            ApplicationUser user = null;
            ImageDetailsViewModel imageDetails = null;

            try
            {
                imageDetails = await galleryService.GetImgDetails(int.Parse(id));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "GalleryController/AddImageToFavourites");
                return DatabaseError();
            }

            try
            {
                try
                {
                    user = await userService.GetUser(userName);
                    await galleryService.AddToFavourites(id, user);
                }
                catch (ArgumentException aex)
                {
                    ViewData[MessageConstant.ErrorMessage] = aex.Message;
                    return View("Details", imageDetails);
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = ErrorMessages.ImageExists;
                return View("Details", imageDetails);
            }

            ViewData[MessageConstant.SuccessMessage] = ConfirmationMessage.ImageAdded;
            return View("Details", imageDetails);
        }

        private IActionResult DatabaseError()
        {
            ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
            return View("_Error", error);
        }
    }
}
