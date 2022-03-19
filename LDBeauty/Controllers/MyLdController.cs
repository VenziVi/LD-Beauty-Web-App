using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class MyLdController : Controller
    {
        public IActionResult Info()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("~/Identity/Account/Login");
            }

            return View();
        }
    }
}
