using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult All()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }
    }
}
