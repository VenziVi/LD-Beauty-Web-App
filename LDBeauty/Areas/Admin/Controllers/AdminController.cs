using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models.User;
using LDBeauty.Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IUserService userService;
        private readonly IOrderService orderService;

        public AdminController(
            IUserService _userService,
            IOrderService _orderService)
        {
            userService = _userService;
            orderService = _orderService;
        }


        public async Task<IActionResult> AllUsers()
        {
            List<AllUsersViewModel> users = null;

            try
            {
                users = await userService.GetAllUsers();
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Something went wrong!";
                return View();
            }

            return View(users);
        }

        public async Task<IActionResult> UserOrders(string id)
        {
            List<UserProductsViewModel> products = null;
          
            ApplicationUser user = await userService.GetUserById(id);

            ViewData["Name"] = $"{user.FirstName} {user.LastName}";

            try
            {
                products = await orderService.GetUserProducts(id);
            }
            catch (Exception)
            {
                ViewData[MessageConstant.ErrorMessage] = "Something went wrong!";
                return View();
            }

            return View(products);
        }
    }
}
