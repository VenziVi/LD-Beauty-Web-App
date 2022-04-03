﻿using LDBeauty.Core.Constants;
using LDBeauty.Core.Constraints;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models;
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
                ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
                return View("_Error", error);
            }

            return View(users);
        }

        public async Task<IActionResult> UserOrders(string id)
        {
            List<UserProductsViewModel> products = null;          
            ApplicationUser user = null;

            try
            {
                user = await userService.GetUserById(id);
                products = await orderService.GetUserProducts(id);
            }
            catch (Exception)
            {
                ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
                return View("_Error", error);
            }

            ViewData["Name"] = $"{user.FirstName} {user.LastName}";
            return View(products);
        }
    }
}
