using LDBeauty.Core.Constants;
using LDBeauty.Core.Constraints;
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

        public GalleryController(
            IGalleryService _galleryService,
            IUserService _userService)
        {
            galleryService = _galleryService;
            userService = _userService;
        }

        public async Task<IActionResult> Category()
        {
            List<GalleryCategoryViewModel> model = null;

            try
            {
                model = await galleryService.GetCategories();
            }
            catch (Exception)
            {
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
                catch (Exception)
                {
                    return DatabaseError();
                }
            }
            else
            {
                try
                {
                    images = await galleryService.GetImages(id);
                }
                catch (Exception)
                {
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
            catch (Exception)
            {
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
            catch (Exception)
            {
                return DatabaseError();
            }

            try
            {
                user = await userService.GetUser(userName);
                await galleryService.AddToFavourites(id, user);
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Image already exists in favourites!";
                return View("Details", imageDetails);
            }

            ViewData[MessageConstant.SuccessMessage] = "Image was added successfuly";
            return View("Details", imageDetails);
        }

        private IActionResult DatabaseError()
        {
            ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
            return View("_Error", error);
        }
    }
}
