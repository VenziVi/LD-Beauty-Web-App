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
        public async Task<IActionResult> Order(string id)
        {
            var userName = User.Identity.Name;

            UserOrderViewModel user = await userService.GetUSerByName(userName, id);

            return View(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FinishOrder(FinishOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData[MessageConstant.ErrorMessage] = "Shipping data is not corect!";
            }

            try
            {
                await cartService.FinishOrder(model);
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Something went wrong, please try again later!";
            }

            //REPAIR
            return View("Detail", ViewData[MessageConstant.SuccessMessage] = "Order confirmed.");
        }

        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                await cartService.DeleteProduct(id);
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Something went wrong, please try again later!";
            }

            return Redirect("/Cart/Detail");
        }
    }
}
