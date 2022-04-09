using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Helpers;
using LDBeauty.Core.Models;
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
            if (AddProductToCart.IsAddedToCart)
            {
                ViewData[MessageConstant.SuccessMessage] = "Product was added successfuly to cart.";
                AddProductToCart.IsAddedToCart = false;
            }

            IEnumerable<GetProductViewModel> products = null;

            try
            {
                products = await productService.GetAllProducts();
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            return View(products);
        }


        public async Task<IActionResult> ProductByCategory(int id)
        {
            IEnumerable<GetProductViewModel> products = null;

            try
            {
                products = await productService.GetProductsByCategory(id);
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            return View("AllProducts", products);
        }

        public async Task<IActionResult> ProductByMake(int id)
        {
            IEnumerable<GetProductViewModel> products = null;

            try
            {
                products = await productService.GetProductsByMake(id);
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            return View("AllProducts", products);
        }



        public async Task<IActionResult> Details(int id)
        {
            ProductDetailsViewModel product = null;

            try
            {
                product = await productService.GetProduct(id);
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            return View(product);
        }

        [Authorize]
        public async Task<IActionResult> AddToCart(AddToCartViewModel model)
        {
            var userName = HttpContext.User.Identity.Name;

            try
            {
                try
                {
                    await cartService.AddToCart(model, userName);
                }
                catch (ArgumentException aex)
                {
                    RedirectToAction("details", model.ProductId);
                }
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            AddProductToCart.IsAddedToCart = true;
            return RedirectToAction("AllProducts");
        }

        [Authorize]
        public async Task<IActionResult> AddProductToFavourites(int id)
        {
            var userName = User.Identity.Name;
            ApplicationUser user = null;
            ProductDetailsViewModel product = null;

            try
            {
                product = await productService.GetProduct(id);
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            try
            {
                try
                {
                    user = await userService.GetUser(userName);
                    await productService.AddToFavourites(id, user);
                }
                catch (ArgumentException aex)
                {
                    ErrorViewModel error = new ErrorViewModel() { ErrorMessage = aex.Message };
                    return View("_Error", error);
                }
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Product already exists in favourites!";
                return View("Details", product);
            }

            ViewData[MessageConstant.SuccessMessage] = "Product was added successfuly";
            return View("Details", product);
        }

        private IActionResult DatabaseError()
        {
            ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
            return View("_Error", error);
        }
    }
}
