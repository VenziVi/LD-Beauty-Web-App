﻿using LDBeauty.Core.Constants;
using LDBeauty.Core.Constraints;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Models;
using LDBeauty.Core.Models.Cart;
using LDBeauty.Core.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LDBeauty.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IUserService userService;

        public OrderController(
            IOrderService _orderService,
            IUserService _userService)
        {
            orderService = _orderService;
            userService = _userService;
        }

        [Authorize]
        public async Task<IActionResult> Order(string id)
        {
            var userName = User.Identity.Name;

            UserOrderViewModel user = null;

            try
            {
                user = await userService.GetUSerByName(userName, id);
            }
            catch (Exception)
            {
                return DatabaseError();
            }

            return View(user);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FinishOrder(FinishOrderViewModel model)
        {

            try
            {
                await orderService.FinishOrder(model);
            }
            catch (ArgumentException ex)
            {
                //NotWorking

                ViewData[MessageConstant.ErrorMessage] = ex.Message;
                View("Order");
            }

            ViewData[MessageConstant.SuccessMessage] = "Order confirmed.";
            //REPAIR
            return Redirect("/Cart/Detail");
        }

        private IActionResult DatabaseError()
        {
            ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
            return View("_Error", error);
        }
    }
}
