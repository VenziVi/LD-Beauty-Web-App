using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Helpers;
using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Gallery;
using LDBeauty.Core.Models.Product;
using LDBeauty.Core.Models.User;
using LDBeauty.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    [Authorize]
    public class MyLdController : Controller
    {
        private readonly IUserService userService;
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly IGalleryService galleryService;

        public MyLdController(
            IUserService _userService,
            IOrderService _orderService,
            IProductService _productService,
            IGalleryService _galleryService)
        {
            userService = _userService;
            orderService = _orderService;
            productService = _productService;
            galleryService = _galleryService;
        }


        public async Task<IActionResult> Info()
        {
            var userName = User.Identity.Name;
            ApplicationUser user = null;
            List <UserProductsViewModel> products = null;

            try
            {
                user = await userService.GetUser(userName);
                products = await orderService.GetUserProducts(user.Id);
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            ViewData["UserName"] = user.FirstName;
            return View(products);
        }

        public async Task<IActionResult> FavouriteProducts()
        {
            if (ProductRemovedFromFavourites.IsRemoved)
            {
                ViewData[MessageConstant.ErrorMessage] = ProductRemovedFromFavourites.Message;
                ProductRemovedFromFavourites.IsRemoved = false;
            }

            var userName = User.Identity.Name;
            List<GetProductViewModel> products = null;

            try
            {
                ApplicationUser user = await userService.GetUser(userName);
                products = await productService.GetFavouriteProducts(user);
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            return View(products);
        }

        public async Task<IActionResult> RemoveProduct(int id)
        {
            var userName = User.Identity.Name;
            ApplicationUser user = null;

            try
            {
                try
                {
                    user = await userService.GetUser(userName);
                    await productService.RemoveFromFavourite(id, user);
                }
                catch (ArgumentException aex)
                {
                    ProductRemovedFromFavourites.Message = aex.Message;
                    ProductRemovedFromFavourites.IsRemoved = true;
                    return Redirect("/MyLd/FavouriteProducts");
                }
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            return Redirect("/MyLd/FavouriteProducts");
        }

        public async Task<IActionResult> FavouriteImages()
        {
            if (ImageRemovedFromFavourites.IsRemoved)
            {
                ViewData[MessageConstant.ErrorMessage] = ImageRemovedFromFavourites.Message;
                ImageRemovedFromFavourites.IsRemoved = false;
            }

            var userName = User.Identity.Name;
            List<ImageViewModel> images = null;

            try
            {
                ApplicationUser user = await userService.GetUser(userName);
                images = await galleryService.GetFavouriteImages(user);
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            return View(images);
        }

        public async Task<IActionResult> RemoveFromFavourites(int id)
        {
            var userName = User.Identity.Name;
            ApplicationUser user = null;

            try
            {
                try
                {
                    user = await userService.GetUser(userName);
                    await galleryService.RemoveFromFavourite(id, user);
                }
                catch (ArgumentException aex)
                {
                    ImageRemovedFromFavourites.Message = aex.Message;
                    ImageRemovedFromFavourites.IsRemoved = true;
                    return Redirect("/MyLd/FavouriteImages");
                }
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            return Redirect("/MyLd/FavouriteImages");
        }

        private IActionResult DatabaseError()
        {
            ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
            return View("_Error", error);
        }
    }
}
