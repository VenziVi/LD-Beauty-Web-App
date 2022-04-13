using LDBeauty.Core.Constants;
using LDBeauty.Core.Contracts;
using LDBeauty.Core.Helpers;
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
        private readonly ILogger<OrderController> logger;

        public OrderController(
            IOrderService _orderService,
            IUserService _userService,
            ILogger<OrderController> _logger)
        {
            orderService = _orderService;
            userService = _userService;
            logger = _logger;
        }

        [Authorize]
        public async Task<IActionResult> Order(int id)
        {
            var userName = User.Identity.Name;

            UserOrderViewModel user = null;

            try
            {
                user = await userService.GetUSerByName(userName, id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "OrderController/Order");
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
                try
                {
                    await orderService.FinishOrder(model);
                }
                catch (ArgumentException ex)
                {
                    OrderOutOfStock.IsOutOfStock = true;
                    OrderOutOfStock.ErrorMsg = ex.Message;
                    return Redirect("/Cart/Detail");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "OrderController/FinishOrder");
                return DatabaseError();
            }
            
            OrderConfirmed.IsConfirmed = true;
            return Redirect("/Cart/Detail");
        }

        private IActionResult DatabaseError()
        {
            ErrorViewModel error = new ErrorViewModel() { ErrorMessage = ErrorMessages.DatabaseConnectionError };
            return View("_Error", error);
        }
    }
}
