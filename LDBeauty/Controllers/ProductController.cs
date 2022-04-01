using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Core.Models.Product;
using LDBeauty.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly ICartService cartService;
        private readonly IUserService userService;


        public ProductController(
            IProductService _productService,
            ICartService _cartService,
            IUserService _userService)
        {
            productService = _productService;
            cartService = _cartService;
            userService = _userService;
        }

        public async Task<IActionResult> AllProducts()
        {
            IEnumerable<GetProductViewModel> products = await productService.GetAllProducts();
            return View(products);
        }

        public async Task<IActionResult> Details(string id)
        {
            ProductDetailsViewModel product = await productService.GetProduct(id);
            return View(product);
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
                ProductDetailsViewModel product = await productService.GetProduct(model.ProductId.ToString());
                ViewData[MessageConstant.ErrorMessage] = "Something went wrong, please try again later!";
                return View("Details", product);
            }

            return RedirectToAction("AllProducts");
        }

        [Authorize]
        public async Task<IActionResult> addProductToFavourites(string id)
        {
            var userName = User.Identity.Name;
            ApplicationUser user = null;
            ProductDetailsViewModel product = await productService.GetProduct(id);

            try
            {
                user = await userService.GetUser(userName);
                await productService.AddToFavourites(id, user);
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Product already exists in favourites!";
                return View("Details", product);
            }

            ViewData[MessageConstant.SuccessMessage] = "Product was added successfuly";
            return View("Details", product);
        }

        [Authorize]
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
    }
}
