using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Helpers;
using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Core.Models.Product;
using LDBeauty.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace LDBeauty.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly ICartService cartService;
        private readonly IUserService userService;
        private readonly ILogger<ProductController> logger;
        private readonly IMemoryCache cache;


        public ProductController(
            IProductService _productService,
            ICartService _cartService,
            IUserService _userService,
            ILogger<ProductController> _logger,
            IMemoryCache _cache)
        {
            productService = _productService;
            cartService = _cartService;
            userService = _userService;
            logger = _logger;
            cache = _cache;
        }

        public async Task<IActionResult> AllProducts()
        {
            if (AddProductToCart.IsAddedToCart)
            {
                ViewData[MessageConstant.SuccessMessage] = ConfirmationMessage.ProductAdded;
                AddProductToCart.IsAddedToCart = false;
            }

            if (SearchedProductNotFound.NotFound)
            {
                ViewData[MessageConstant.ErrorMessage] = SearchedProductNotFound.Message;
                SearchedProductNotFound.NotFound = false;
                SearchedProductNotFound.Message = string.Empty;
            }

            const string allProductsCacheKey = "AllProductsCacheKey";

            AllProductsViewModel products = cache.Get<AllProductsViewModel>(allProductsCacheKey);

            if (products == null)
            {
                try
                {
                    products = await productService.GetAllProducts();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "ProductController/AllProducts");
                    return DatabaseError();
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                cache.Set(allProductsCacheKey, products, cacheOptions);
            }

            return View(products);
        }


        public async Task<IActionResult> ProductByCategory(int id)
        {
            AllProductsViewModel products = cache.Get<AllProductsViewModel>($"ProductsByCategory{id}");

            if (products == null)
            {
                try
                {
                    products = await productService.GetProductsByCategory(id);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "ProductController/ProductByCategory");
                    return DatabaseError();
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                cache.Set($"ProductsByCategory{id}", products, cacheOptions);
            }

            return View("AllProducts", products);
        }

        public async Task<IActionResult> ProductByMake(int id)
        {
            AllProductsViewModel products = cache.Get<AllProductsViewModel>($"ProductsByMake{id}");

            if (products == null)
            {
                try
                {
                    products = await productService.GetProductsByMake(id);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "ProductController/ProductByMake");
                    return DatabaseError();
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                cache.Set($"ProductsByMake{id}", products, cacheOptions);
            }

            return View("AllProducts", products);
        }



        public async Task<IActionResult> Details(int id)
        {
            ProductDetailsViewModel product = cache.Get<ProductDetailsViewModel>($"ProductsDetails{id}");

            if (product == null)
            {
                try
                {
                    product = await productService.GetProduct(id);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "ProductController/Details");
                    return DatabaseError();
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                   .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                cache.Set($"ProductsDetails{id}", product, cacheOptions);
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
            catch (Exception ex)
            {
                logger.LogError(ex, "ProductController/AddToCart");
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
            catch (Exception ex)
            {
                logger.LogError(ex, "ProductController/AddProductToFavourites");
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
                ViewData[MessageConstant.ErrorMessage] = ErrorMessages.ProductEists;
                return View("Details", product);
            }

            ViewData[MessageConstant.SuccessMessage] = ErrorMessages.ProductAddedToFavoutites;
            return View("Details", product);
        }

        public async Task<IActionResult> SearchByName(string productName)
        {

            if (productName == null || productName.Length <= 3)
            {
                SearchedProductNotFound.NotFound = true;
                SearchedProductNotFound.Message = ErrorMessages.MinCharacters;
                return RedirectToAction("AllProducts");
            }

            AllProductsViewModel products = null;

            try
            {
                products = await productService.GetProductsByName(productName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "ProductController/SearchByName");
                return DatabaseError();
            }

            if (products.Products.Count() == 0)
            {
                SearchedProductNotFound.NotFound = true;
                SearchedProductNotFound.Message = ErrorMessages.MissingProduct;
                return RedirectToAction("AllProducts");
            }

            return View("AllProducts", products);
        }

        private IActionResult DatabaseError()
        {
            ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
            return View("_Error", error);
        }
    }
}
