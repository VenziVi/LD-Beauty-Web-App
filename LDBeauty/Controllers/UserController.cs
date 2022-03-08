using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
