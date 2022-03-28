using LDBeauty.Core.Models.Cart;
using LDBeauty.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDBeauty.Core.Contracts
{
    public interface IOrderService
    {
        Task FinishOrder(FinishOrderViewModel model);
        Task<List<UserProductsViewModel>> GetUserProducts(string userId);
    }
}
