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
        

        public CartController(
            ICartService _cartService)
        {
            cartService = _cartService;
        }


        [Authorize]
        public async Task<IActionResult> Detail()
        {
            var userName = User.Identity.Name;
            CartDetailsViewModel cart = null;

            try
            {
                cart = await cartService.GetCart(userName);
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Something went wrong!";
            }

            return View(cart);
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
