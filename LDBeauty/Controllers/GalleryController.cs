﻿using LDBeauty.Core.Constants;
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

        public IActionResult Category()
        {
            IEnumerable<GalleryCategoryViewModel> model = galleryService.GetMCategories();

            return View(model);
        }

        public IActionResult Images(int? id)
        {
            if (id == null)
            {
                IEnumerable<ImageViewModel> images = galleryService.AllImages();

                return View(images);
            }

            IEnumerable<ImageViewModel> currImages = galleryService.GetImages(id);

            return View(currImages);
        }

        public IActionResult Details(int id)
        {
            ImageDetailsViewModel imageDetails = galleryService.GetImgDetails(id);

            return View(imageDetails);
        }

        [Authorize]
        public async Task<IActionResult> AddImageToFavourites(string id)
        {
            var userName = User.Identity.Name;
            ApplicationUser user = null;
            ImageDetailsViewModel imageDetails = galleryService.GetImgDetails(int.Parse(id));

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
    }
}
