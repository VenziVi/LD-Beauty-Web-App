using LDBeauty.Core.Contracts;
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

        public MyLdController(
            IUserService _userService,
            IOrderService _orderService,
            IProductService _productService)
        {
            userService = _userService;
            orderService = _orderService;
            productService = _productService;
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
    }
}
