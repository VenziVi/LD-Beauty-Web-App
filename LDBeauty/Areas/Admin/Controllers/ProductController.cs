using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        public IActionResult AddProduct()
        {
            return View();
        }
    }
}
