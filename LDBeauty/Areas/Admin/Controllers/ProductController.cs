using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Product;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService productService;

        public ProductController(IProductService _productService)
        {
            productService = _productService;
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData[MessageConstant.ErrorMessage] = "Data is not correct!";
                return View();
            }

            try
            {
                await productService.AddProduct(model);
            }
            catch (Exception)
            {

                ViewData[MessageConstant.ErrorMessage] = "Something went wrong!";
                return View();
            }

            return RedirectToAction("AddProduct");
        }
    }
}
