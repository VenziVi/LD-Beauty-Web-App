using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Type()
        {
            return View();
        }

        public IActionResult GalleryTemp()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}
