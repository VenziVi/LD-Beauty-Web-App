using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult All()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}
