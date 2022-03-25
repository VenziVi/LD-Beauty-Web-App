using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService cartService;

        public CartController(ICartService _cartService)
        {
            cartService = _cartService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddToCartViewModel model)
        {

            try
            {
                await cartService.AddToCart(model);
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Something went wrong!";
            }

            ViewData[MessageConstant.SuccessMessage] = "Product was added to cart!";
            return View("/Product/AllProduct");
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
