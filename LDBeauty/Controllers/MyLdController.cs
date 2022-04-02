using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Gallery;
using LDBeauty.Core.Models.Product;
using LDBeauty.Core.Models.User;
using LDBeauty.Core.Services;
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
            ApplicationUser user = await userService.GetUser(userName);
            ViewData["UserName"] = user.FirstName;

            List<UserProductsViewModel> products = await orderService.GetUserProducts(user.Id);
            return View(products);
        }

        public async Task<IActionResult> FavouriteProducts()
        {
            var userName = User.Identity.Name;
            ApplicationUser user = await userService.GetUser(userName);

            List<GetProductViewModel> products = await productService.GetFavouriteProducts(user);

            return View(products);
        }

        public async Task<IActionResult> RemoveProduct(string id)
        {
            var userName = User.Identity.Name;
            ApplicationUser user = null;

            try
            {
                user = await userService.GetUser(userName);
                await productService.RemoveFromFavourite(id, user);
            }
            catch (Exception)
            {

                throw;
            }

            return Redirect("/MyLd/FavouriteProducts");
        }

        public async Task<IActionResult> FavouriteImages()
        {
            var userName = User.Identity.Name;
            ApplicationUser user = await userService.GetUser(userName);

            IEnumerable<ImageViewModel> images = await galleryService.GetFavouriteImages(user);

            return View(images);
        }

        public async Task<IActionResult> RemoveFromFavourites(int id)
        {
            var userName = User.Identity.Name;
            ApplicationUser user = null;

            try
            {
                user = await userService.GetUser(userName);
                await galleryService.RemoveFromFavourite(id, user);
            }
            catch (Exception)
            {

                throw;
            }

            return Redirect("/MyLd/FavouriteImages");
        }
    }
}
