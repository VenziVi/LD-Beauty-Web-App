using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Core.Models.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        private readonly IUserService userService;

        public CartController(
            ICartService _cartService,
            IUserService _userService)
        {
            cartService = _cartService;
            userService = _userService;
        }

        [Authorize]
        public async Task<IActionResult> Add(AddToCartViewModel model)
        {
            var userName = HttpContext.User.Identity.Name;

            try
            {
                await cartService.AddToCart(model, userName);
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Something went wrong!";
            }

            //ViewData[MessageConstant.SuccessMessage] = "Product was added to cart!";
            return Redirect("/Product/AllProducts");
        }

        [Authorize]
        public async Task<IActionResult> Detail()
        {
            CartDetailsViewModel cart = null;

            try
            {
                cart = await cartService.GetCart();
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Something went wrong!";
            }

            return View(cart);
        }

        [Authorize]
        public async Task<IActionResult> Order(string cartId)
        {
            var userName = User.Identity.Name;

            UserOrderViewModel user = await userService.GetUSerByName(userName, cartId);

            return View(user);
        }
    }
}
