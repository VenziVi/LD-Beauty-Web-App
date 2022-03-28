using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.User;
using LDBeauty.Core.Services;
using LDBeauty.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class MyLdController : Controller
    {
        private readonly IUserService userService;
        private readonly IOrderService orderService;

        public MyLdController(
            IUserService _userService,
            IOrderService _orderService)
        {
            userService = _userService;
            orderService = _orderService;
        }


        [Authorize]
        public async Task<IActionResult> Info()
        {
            var userName = User.Identity.Name;
            ApplicationUser user = await userService.GetUser(userName);
            ViewData["UserName"] = user.FirstName;

            List<UserProductsViewModel> products = await orderService.GetUserProducts(user.Id);
            return View(ViewData);
        }
    }
}
