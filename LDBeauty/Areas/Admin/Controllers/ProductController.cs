using LDBeauty.Core.Constants;
using LDBeauty.Core.Constraints;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Helpers;
using LDBeauty.Core.Models;
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
            if (ProductAdding.isAdded)
            {
                ViewData[MessageConstant.SuccessMessage] = "Product was added successfuly";
                ProductAdding.isAdded = false;
            }

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
                return DatabaseError();
            }

            ProductAdding.isAdded = true;
            return Redirect("AddProduct");
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
                return DatabaseError();
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
                return DatabaseError();
            }

            ViewData[MessageConstant.SuccessMessage] = "Product was added successfuly";
            return View();
        }

        private IActionResult DatabaseError()
        {
            ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
            return View("_Error", error);
        }
    }
}
