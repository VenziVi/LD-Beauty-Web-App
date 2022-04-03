using LDBeauty.Core.Constraints;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService cartService;
        

        public CartController(
            ICartService _cartService)
        {
            cartService = _cartService;
        }


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
                return DatabaseError();
            }

            return View(cart);
        }


        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                await cartService.DeleteProduct(id);
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            return Redirect("/Cart/Detail");
        }

        private IActionResult DatabaseError()
        {
            ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
            return View("_Error", error);
        }
    }
}
