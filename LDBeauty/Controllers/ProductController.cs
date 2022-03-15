using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult AllProducts()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult AddProduct()
        {
            return View();
        }

    }
}
