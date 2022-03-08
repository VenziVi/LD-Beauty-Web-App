using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Detail()
        {
            return View();
        }
    }
}
