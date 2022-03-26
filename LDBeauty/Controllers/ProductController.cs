using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Core.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly ICartService cartService;

        public ProductController(IProductService _productService,
            ICartService _cartService)
        {
            productService = _productService;
            cartService = _cartService;
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
    }
}
