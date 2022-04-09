using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Helpers;
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
            if (OrderConfirmed.IsConfirmed)
            {
                ViewData[MessageConstant.SuccessMessage] = ConfirmationMessage.OrderConfirmed;
                OrderConfirmed.IsConfirmed = false;
            }

            if (CartProductDelete.IsDeleted)
            {
                ViewData[MessageConstant.SuccessMessage] = ConfirmationMessage.ProductDeleted;
                CartProductDelete.IsDeleted = false;
            }

            if (OrderOutOfStock.IsOutOfStock)
            {
                ViewData[MessageConstant.ErrorMessage] = OrderOutOfStock.ErrorMsg;
                OrderOutOfStock.IsOutOfStock = false;
            }

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


        public async Task<IActionResult> Delete(int id)
        {

            var userName = User.Identity.Name;

            try
            {
                await cartService.DeleteProduct(id, userName);
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            CartProductDelete.IsDeleted = true;
            return Redirect("/Cart/Detail");
        }

        private IActionResult DatabaseError()
        {
            ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
            return View("_Error", error);
        }
    }
}
