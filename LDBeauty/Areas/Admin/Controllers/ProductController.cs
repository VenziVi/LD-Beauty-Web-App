using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Core.Models.Product;
using LDBeauty.Infrastructure.Data.Identity;
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
            ViewData[MessageConstant.SuccessMessage] = "Product was added successfuly";

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

            ViewData[MessageConstant.SuccessMessage] = "Product was added successfuly";
            return View("/Admin/Product/AddProduct");
        }

        public async Task<IActionResult> EditProduct(string id)
        {

            ProductDetailsViewModel product = null;

            try
            {
                product = await productService.GetProduct(id);
            }
            catch (Exception)
            {

                throw;
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(AddProductViewModel model, string id)
        {

            if (!ModelState.IsValid)
            {
                TempData[MessageConstant.ErrorMessage] = "Data is not correct!";
                return View();
            }

            try
            {
                await productService.EditProduct(model, id);
            }
            catch (Exception)
            {

                TempData[MessageConstant.ErrorMessage] = "Something went wrong!";
                return View();
            }

            ViewData[MessageConstant.SuccessMessage] = "Product was added successfuly";
            return RedirectToAction();
        }
    }
}
